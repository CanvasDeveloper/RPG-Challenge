using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum EnemyState
{
    PATROL, ATTACK, DEAD
}

[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour, IHealthSystem
{
    [SerializeField]private EnemyState enemyState;
    [SerializeField]private Transform shotPoint;
    private NavMeshAgent agent;
    private HealthSystem healthSystem;
    private ShotSystem shotSystem;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        shotSystem = GetComponent<ShotSystem>();
        healthSystem = GetComponent<HealthSystem>();
        StartCoroutine(testeTiro());
    }

    public void GetHit(int damage)
    {
        healthSystem.Damage(damage);
    }

    public void Death()
    {

    }

    IEnumerator testeTiro()
    {
        yield return new WaitForSeconds(1f);
        shotSystem.Fire(shotPoint);
        StartCoroutine(testeTiro());
    }
}
