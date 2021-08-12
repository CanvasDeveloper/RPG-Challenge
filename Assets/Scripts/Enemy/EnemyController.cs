using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStateController))]
[RequireComponent(typeof(ShotSystem))]
[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour, IHealthSystem
{
    private CharacterController character;
    private EnemyStateController enemyState;
    private RayCastCheckTarget rayCastCheckTarget;
    private HealthSystem healthSystem;
    [SerializeField]private ShotSystem shotSystem1;
    [SerializeField]private ShotSystem shotSystem2;
    private Animator animator;
    [SerializeField]private Transform shotPoint;
    [HideInInspector]public NavMeshAgent agent;

    [Header("Enemy Behaviour")]
    [SerializeField]private float timeInIdle;
    [SerializeField]private Transform[] waypoints;
    bool isAttacking;
    bool isDead;

    [Header("NavMesh Agent Settings")]
    [SerializeField]private float patrolStopDistance = 0f;
    [SerializeField]private float chaseStopDistance = 4f;
    [SerializeField]private float attackDistanceOffset = 2f; 
    [SerializeField]private bool isRandomWaypointPatrol;
    private int idWaypoint;
    private Transform target;
    private Vector3 playerPos;

    [Header("Enemy VFX")]
    [SerializeField]private ParticleSystem darkEffectParticle;
    [SerializeField]private SkinnedMeshRenderer mesh;
    [SerializeField]private Material dissolveMaterial;
    private float dissolveFloat;

    private void Start()
    {
        enemyState = GetComponent<EnemyStateController>();
        healthSystem = GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
        character = GetComponent<CharacterController>();
        rayCastCheckTarget = GetComponentInChildren<RayCastCheckTarget>();
        agent = GetComponent<NavMeshAgent>();

        target = waypoints[idWaypoint];
        ChangeState(EnemyState.IDLE);
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        playerPos = EnemyDataBase.Instance.player.transform.position;

        CheckState();
    }

    void CheckState()
    {
        if(isDead && enemyState.currentState != EnemyState.DEAD)
        {
            enemyState.currentState = EnemyState.DEAD;
            StartCoroutine(Dead());
        }

        if(!isDead && rayCastCheckTarget.target != null && enemyState.currentState != EnemyState.CHASE)
        {
            ChangeState(EnemyState.CHASE);
        }
    }

    void ApplyGravity()
    {
        if(!character.isGrounded)
        {
            character.Move(new Vector3(0, Physics.gravity.y * Time.fixedDeltaTime, 0));
        }
    }

    void Attack1()
    {
        darkEffectParticle.Play();
        animator.SetTrigger("Attack1");
        shotSystem1.Fire(shotPoint);
    }

    void Attack2()
    {
        darkEffectParticle.Play();
        animator.SetTrigger("Attack1");
        shotSystem2.Fire(shotPoint);
    }

    IEnumerator InstantiateAttackDelay(Vector3 position)
    {
        yield return null;
    }

    public void FinishAttack() //called by animator on exit animation
    {
        isAttacking = false;
    }

    #region State

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(timeInIdle);
        ChangeState(EnemyState.PATROL);
    }

    IEnumerator Patrol()
    {
        agent.destination = target.position;

        yield return new WaitUntil(() => CheckStoppingDistance());
        
        if(isRandomWaypointPatrol)
        {
            int prevWaypoint = idWaypoint;
            idWaypoint = IntRand(waypoints.Length);
            if(idWaypoint == prevWaypoint)
            {
                idWaypoint ++;
                if(idWaypoint >= waypoints.Length) {idWaypoint = 0;}
            }
        }
        else
        {
            idWaypoint ++;
            if(idWaypoint >= waypoints.Length) {idWaypoint = 0;}
        }
        target = waypoints[idWaypoint];

        yield return new WaitUntil(() => CheckStoppingDistance());

        ChangeState(EnemyState.IDLE);
    }

    IEnumerator Chase()
    {
        if(enemyState.currentState != EnemyState.DEAD)
        {
            agent.stoppingDistance = chaseStopDistance;
            agent.destination = playerPos;
        
            if(!CheckStoppingDistance()) { SetAnimatorState((int)EnemyState.CHASE); }

            yield return new WaitUntil(() => CheckStoppingDistance());

            if(!isAttacking)
            {
                isAttacking = true;
                if(BoolRand(50f))
                {
                    Attack1();
                }
                else
                {
                    Attack2();
                }
            }

            Vector3 direction = playerPos - transform.position;
            direction.Set(direction.x, 0f, direction.z);
            transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);
            SetAnimatorState(0);
            
            StartCoroutine(Chase());
        }
    }

    IEnumerator Victory()
    {
        yield return null;
    }

    #endregion
    bool CheckStoppingDistance()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    public bool BoolRand(float chance)
    {
        return Random.Range(0f, 100f) < chance;
    }

    public int IntRand(int maxInterger)
    {
        return Random.Range(0, maxInterger);
    }

    #region DAMAGE_REGION
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

    IEnumerator Dead()
    {
        yield return new WaitForEndOfFrame();
        SetDissolve();
        StartCoroutine(Dead());
    }

    public void GetHit(int damage)
    {
        if(enemyState.currentState == EnemyState.DEAD) { return; }
        animator.SetTrigger("GetHit");
        healthSystem.Damage(damage);
        if(enemyState.currentState != EnemyState.CHASE)
        {
            ChangeState(EnemyState.CHASE);
        }
    }

    public void Death()
    {
        isDead = true;
        if(!agent.isStopped)
        {
            agent.isStopped = true;
            character.detectCollisions = false;
        }
        mesh.material = dissolveMaterial;
        mesh.material.SetFloat("_Dissolve", 0f);
        animator.SetTrigger("Death");
    }

    #endregion
    void SetAnimatorState(int newState)
    {
        animator.SetInteger("state", newState);
    }

    void ChangeState(EnemyState newState)
    {
        enemyState.currentState = newState;
        StopAllCoroutines();
        
        if(enemyState.currentState != EnemyState.DEAD)
        {
            switch(enemyState.currentState)
            {
            case EnemyState.IDLE:
                StartCoroutine(Idle());
            break;

            case EnemyState.PATROL:
                StartCoroutine(Patrol());
            break;

            case EnemyState.CHASE:
                StartCoroutine(Chase());
            break;

            case EnemyState.VICTORY:
                StartCoroutine(Victory());
            break;
            }

            SetAnimatorState((int)enemyState.currentState);
        }
    }
}
