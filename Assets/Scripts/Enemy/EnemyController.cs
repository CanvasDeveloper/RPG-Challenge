using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


[RequireComponent(typeof(EnemyStateController))]
[RequireComponent(typeof(ShotSystem))]
[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour, IHealthSystem
{
    private EnemyStateController enemyState;
    private NavMeshAgent agent;
    private HealthSystem healthSystem;
    private ShotSystem shotSystem;

    [SerializeField]private Transform shotPoint;
    [Header("Enemy VFX")]
    [SerializeField]private SkinnedMeshRenderer mesh;
    [SerializeField]private Material dissolveMaterial;
    private float dissolveFloat = 0;

    private void Start()
    {
        enemyState = GetComponent<EnemyStateController>();
        agent = GetComponent<NavMeshAgent>();
        shotSystem = GetComponent<ShotSystem>();
        healthSystem = GetComponent<HealthSystem>();
        StartCoroutine(testeTiro());
    }

    private void Update()
    {
        SetDissolve();
    }

    void SetDissolve()
    {
        if(enemyState.currentState == EnemyState.DEAD && dissolveFloat < 1)
        {
            dissolveFloat += 0.01f;
            mesh.material.SetFloat("_Dissolve", dissolveFloat);
        }
        else if(enemyState.currentState == EnemyState.DEAD && dissolveFloat >= 1)
        {
            Destroy(gameObject);
        }
    }   

    public void GetHit(int damage)
    {
        healthSystem.Damage(damage);
    }

    public void Death()
    {
        ChangeState(EnemyState.DEAD);
    }

    IEnumerator testeTiro()
    {
        yield return new WaitForSeconds(1f);
        shotSystem.Fire(shotPoint);
        StartCoroutine(testeTiro());
    }
    void ChangeState(EnemyState newState)
    {
        enemyState.currentState = newState;

        switch(enemyState.currentState)
        {
            case EnemyState.PATROL:

            break;

            case EnemyState.ATTACK:

            break;

            case EnemyState.DEAD:
                mesh.material = dissolveMaterial;
                mesh.material.SetFloat("_Dissolve", 0f);
            break;
        }
    }
}
