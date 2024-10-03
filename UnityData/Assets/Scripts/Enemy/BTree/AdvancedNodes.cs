using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    public class CheckAllies : Node
    {
        readonly Transform transform;
        readonly float radius;
        readonly LayerMask layerMask;

        public CheckAllies(Transform in_transform, float radius, LayerMask layer)
        {
            transform = in_transform;
            this.radius = radius;
            layerMask = layer;
        }

        public override NodeState Evaluate()
        {
            Collider[] allies = new Collider[3];
            List<GameObject> allies2 = new();
            int n = Physics.OverlapSphereNonAlloc(transform.position, radius, allies, layerMask);

            for (int i = 0; i < n; i++)
            {
                allies2.Add(allies[i].gameObject);
            }

            if (allies2.Count > 0)
            {
                blackboard.AddToBlackboard("allies", allies2);
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }

    public class IsAnyoneHurt : Node
    {
        public IsAnyoneHurt()
        {
        }

        public override NodeState Evaluate()
        {
            blackboard.TryGetFromBlackboard("allies", out List<GameObject> allies);
            if (allies == null || allies.Count == 0)
                return NodeState.Failure;

            foreach (var ally in allies)
            {
                if (!ally) continue;
                if (!ally.TryGetComponent(out EnemyAI ai)) continue;
                if (!ai.BlackBoard.TryGetFromBlackboard("hurt", out bool hurt)) continue;
                if (hurt)
                {
                    blackboard.AddToBlackboard("hurt_ally", true);
                    return NodeState.Success;
                }
            }
            return NodeState.Failure;
        }
    }

    public class CanSeePlayer : Node
    {
        readonly Transform transform;
        readonly Transform player;
        readonly float sightRange;
        readonly float viewAngle;

        public CanSeePlayer(Transform in_transform, Transform player, float sightRange, float viewAngle)
        {
            transform = in_transform;
            this.player = player;
            this.sightRange = sightRange;
            this.viewAngle = viewAngle;
        }

        public override NodeState Evaluate()
        {
            var p = player.position;
            p = new Vector3(p.x, p.y + 0.75f, p.z);
            var direction = p - transform.position;
            if (Vector3.Distance(transform.position, player.position) > sightRange)
                return NodeState.Failure;

            var angle = Vector3.Angle(direction, transform.forward);
            if (angle > viewAngle / 2)
                return NodeState.Failure;

            if (!Physics.Raycast(transform.position + direction.normalized * 1.5f, direction.normalized, out var hit, sightRange))
                return NodeState.Failure;

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Spotted");
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }

    public class CanAttack : Node
    {
        readonly Transform transform;
        readonly Transform player;
        readonly float attackRange;

        public CanAttack(Transform in_transform, Transform player, float attackRange)
        {
            transform = in_transform;
            this.player = player;
            this.attackRange = attackRange;
        }

        public override NodeState Evaluate()
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                Debug.Log("Attacking");
                blackboard.AddToBlackboard("state", EnemyState.ATTACK);
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }

    public class CanChase : Node
    {
        readonly Transform transform;
        readonly Transform player;
        readonly float range;

        public CanChase(Transform in_transform, Transform player, float range)
        {
            transform = in_transform;
            this.player = player;
            this.range = range;
        }

        public override NodeState Evaluate()
        {
            if (Vector3.Distance(transform.position, player.position) <= range)
            {
                Debug.Log("Chasing");
                blackboard.AddToBlackboard("state", EnemyState.CHASE);
                return NodeState.Success;
            }
            return NodeState.Failure;
        }
    }

    public class Investigate : Node
    {
        readonly Transform player;
        public Investigate(Transform player)
        {
            this.player = player;
        }

        public override NodeState Evaluate()
        {
            blackboard.AddToBlackboard("last_player_pos", player.position);
            blackboard.AddToBlackboard("state", EnemyState.INVESTIGATE);

            Debug.Log("Investigating");
            return NodeState.Success;
        }
    }

    public class HasInterest : Node
    {
        public override NodeState Evaluate()
        {
            if (blackboard.TryGetFromBlackboard("interest", out bool val) && val)
            {
                // Attack and interest meaning the player is not in sight but the enemy knows the player is around
                if (blackboard.TryGetFromBlackboard("state", out EnemyState state) && state == EnemyState.ATTACK)
                    blackboard.AddToBlackboard("state", EnemyState.CHASE);
                return NodeState.Success;
            }

            Debug.Log("No interest");
            return NodeState.Failure;
        }
    }

    public class ResetInterest : Node
    {
        public override NodeState Evaluate()
        {
            blackboard.AddToBlackboard("reset_interest", true);
            return NodeState.Success;
        }
    }
}