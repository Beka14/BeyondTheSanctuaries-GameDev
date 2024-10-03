using Behavior;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    INVESTIGATE
}

public class EnemyAI : MonoBehaviour
{
    CancellationTokenSource cts;

    [SerializeField] private SoldierScriptable soldierScriptable;

    [Header("Navmesh Agent")]
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField] private Transform player;

    [Header("Patrol type")]
    [SerializeField]
    private bool randomPatrol;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float idleTime;
    [SerializeField] private int currentPatrolPoint;
    private float _idleTimer;
    private bool _patrolPointSet;

    [Header("Attack")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    [Header("Ranges")]
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private float sightRange;
    [SerializeField] private float investigationRange;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float investigationTime;
    [SerializeField] private float hurtTime = 3;

    [Header("Enemy visuals")]
    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField] private Animator animator;
    [SerializeField] private EnemyState currentState;

    // BTree
    BlackBoard blackBoard = new();
    Node bTree;

    [Header("BTree")]
    [SerializeField] private float awarenessRange = 5;
    [SerializeField] private float reactionTime = 0.5f;

    // State
    float hurtTimer = 0;
    float interestTimer = 0;

    public BlackBoard BlackBoard { get => blackBoard; }

    // ------------------------------

    private void Awake()
    {
        player = GameObject.Find("Character_American_Soldier_02").transform;
        agent = GetComponent<NavMeshAgent>();
        var soldier = GetComponent<Soldier>();
        soldier.OnHurt += SetHurt;

        bTree = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CanSeePlayer(headTransform, player, sightRange, viewAngle),
                    new ResetInterest(), // Actions are not driven by the interest
                    new Selector(new List<Node>
                    {
                        new CanAttack(transform, player, attackRange),
                        new CanChase(transform, player, investigationRange),
                        new Investigate(player),
                    }),
                }),
                new HasInterest(), // If the enemy has interest, it will chase the player
                new Sequence(new List<Node>
                {
                    new CheckAllies(transform, awarenessRange, 1 << gameObject.layer),
                    new IsAnyoneHurt(),
                    new Investigate(player),
                }),
            });
        bTree.SetBlackBoard(blackBoard);

        blackBoard.AddToBlackboard("hurt", false);
        blackBoard.AddToBlackboard("interest", false);
        blackBoard.AddToBlackboard("state", EnemyState.IDLE);
    }

    private void OnEnable()
    {
        cts = new CancellationTokenSource();
        _ = EvaluateState(cts.Token);
    }

    private void OnDisable()
    {
        cts.Cancel();
        cts.Dispose();
    }

    private void Update()
    {
        EnemyAction(currentState);

        if (hurtTimer > 0)
            hurtTimer -= Time.deltaTime;

        if (interestTimer > 0)
            interestTimer -= Time.deltaTime;
    }

    private async UniTaskVoid EvaluateState(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            // A bit overshooting, but it's fine as long as it is not smashing animator values
            if (hurtTimer < 0)
            {
                blackBoard.AddToBlackboard("hurt", false);
                hurtTime = 0;
            }

            if (interestTimer < 0)
            {
                blackBoard.AddToBlackboard("interest", false);
                interestTimer = 0;
            }

            var newState = GetState();
            if (newState != currentState)
            {
                currentState = newState;
                TranslateState(newState);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(reactionTime), cancellationToken: cancellation);
        }
    }

    // Hurt behaviour
    private void SetHurt()
    {
        blackBoard.AddToBlackboard("hurt", true);
        hurtTimer = hurtTime;
    }

    protected void SetInterest()
    {
        blackBoard.AddToBlackboard("interest", true);
        interestTimer = investigationTime;
    }

    private void TranslateState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
                animator.SetBool("patrol", false);
                animator.SetBool("aim", false);
                return;
            case EnemyState.PATROL:
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
                animator.SetBool("patrol", true);
                animator.SetBool("aim", false);
                return;
            case EnemyState.CHASE:
                animator.SetBool("walk", false);
                animator.SetBool("run", true);
                animator.SetBool("patrol", false);
                animator.SetBool("aim", false);
                break;
            case EnemyState.ATTACK:
                animator.SetBool("walk", false);
                animator.SetBool("run", true);
                animator.SetBool("patrol", false);
                animator.SetBool("aim", true);
                break;
            case EnemyState.INVESTIGATE:
                animator.SetBool("walk", true);
                animator.SetBool("run", false);
                animator.SetBool("patrol", false);
                animator.SetBool("aim", false);
                break;
        }

        // Reset interest
        if (blackBoard.TryGetFromBlackboard("reset_interest", out bool interest) && interest)
            SetInterest();
    }

    // State machine
    private EnemyState GetState()
    {
        var state = bTree.Evaluate();
        bool has_state = blackBoard.TryGetFromBlackboard("state", out EnemyState enemyState);

        if (state == NodeState.Failure || !has_state)
        {
            if(blackBoard.TryGetFromBlackboard("hurt", out bool hurt) && hurt)
            {
                SetHurt();
                return EnemyState.CHASE;
            }
            return patrolPoints.Length > 0
                ? EnemyState.PATROL
                : EnemyState.IDLE;
        }

        return enemyState;
    }

    private void EnemyAction(EnemyState newState)
    {
        switch (newState)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.INVESTIGATE:
                Investigate();
                break;
            case EnemyState.CHASE:
                Debug.Log("Chasing Y");
                Chase();
                break;
            case EnemyState.ATTACK:
                Attack();
                break;
            case EnemyState.IDLE:
                Idle();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Idle()
    {
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        agent.speed = 0;
    }

    private void SetPatrolPoint()
    {
        if (randomPatrol)
        {
            currentPatrolPoint = Random.Range(0, patrolPoints.Length);
        }
        else
        {
            currentPatrolPoint++;
            currentPatrolPoint %= patrolPoints.Length;
        }

        _idleTimer = idleTime;
        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        agent.speed = soldierScriptable.walkSpeed;

        _patrolPointSet = true;
    }


    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        agent.isStopped = false;
        if (!_patrolPointSet)
        {
            animator.SetBool("patrol", true);
            SetPatrolPoint();
        }

        if (agent.destination != patrolPoints[currentPatrolPoint].position)
        {
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) > 0.35f) return;

        if (_idleTimer > 0)
        {
            _idleTimer -= Time.deltaTime;
            animator.SetBool("patrol", false);
        }
        else
        {
            _patrolPointSet = false;
        }
    }

    private void Investigate()
    {
        if (!blackBoard.TryGetFromBlackboard("last_player_pos", out Vector3 pos))
            return;

        agent.isStopped = false;
        agent.SetDestination(pos);
        agent.speed = soldierScriptable.walkSpeed;
    }

    private void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        agent.speed = soldierScriptable.runSpeed;

        Debug.Log("Chasing X");
    }

    private void Attack()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        transform.LookAt(player);
        animator.SetBool("shoot", false);

        if (!alreadyAttacked)
        {
            // Attack code here
            alreadyAttacked = true;

            var bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 60f, ForceMode.Impulse);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Debug.Log("Attacking");
            animator.SetBool("shoot", true);
            muzzleFlash.Play();
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, investigationRange);

        // Draw the view distance sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw the view angle lines
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * sightRange;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * sightRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}