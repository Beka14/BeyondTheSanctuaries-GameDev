using System;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    [SerializeField] private SoldierScriptable soldierScriptable;
    [SerializeField] private GameObject dropOnDeath;
    
    private HealthComponent health;
    private Ragdoll ragdoll;
    private EnemyAI enemyAI;
    private NavMeshAgent navMeshAgent;

    public Action OnHurt;

    private void OnEnable()
    {
        health.OnDeath += Die;
    }

    private void OnDisable()
    {
        health.OnDeath -= Die;
    }

    private void Awake()
    {
        health = GetComponent<HealthComponent>();
        ragdoll = GetComponent<Ragdoll>();
        enemyAI = GetComponent<EnemyAI>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health.CurrentHealth -= 20;
            OnHurt?.Invoke();
        }
        if (collision.gameObject.CompareTag("Hand"))
        {
            health.CurrentHealth -= 1000;
            OnHurt?.Invoke();
        }
        if(collision.gameObject.CompareTag("Player"))
        {
            OnHurt?.Invoke();
        }
    }
    
    private void Die()
    {
        ragdoll.ActivateRagdoll();
        GetComponent<Collider>().enabled = false;
        enemyAI.enabled = false;
        navMeshAgent.enabled = false;
        Instantiate(dropOnDeath, transform.position, Quaternion.identity);
    }
}
