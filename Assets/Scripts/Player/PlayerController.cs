using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour, IHealthSystem
{
    private Camera mainCamera;
    private CharacterController character;
    private Animator playerAnimator;
    private PlayerAimCheckCollision aimCheckCollision;
    private ShotSystem shotSystem;
    private HealthSystem healthSystem;

    [Header("Player Settings")]
    [SerializeField]private BoxCollider shieldCollider;
    [SerializeField]private float gravity = -9.81f;
    [SerializeField]private float stepSpeed;
    [SerializeField]private float turnSpeed = 15f;
    [SerializeField]private float delayAttackOneSpearEffect = 0.5f;
    [SerializeField]private float delayToAttackOne = 0.4f;
    private float animSmoothIncrement = 0;
    private Vector3 inputMove;

    [Header("Player Aim")]
    [SerializeField]private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField]private Transform defaultTarget;
    private Transform target;
    private bool isLookAtTarget;
    private bool isWalking;
    private bool isAttacking;
    private bool isDefending;

    [Header("Player Power Particles")]
    [SerializeField] private ParticleSystem fireEffectParticle;
    [SerializeField] private ParticleSystem fireSparklesParticle;
    [SerializeField] private ParticleSystem attack1SpearParticle;

    [Header("Player Spawn Particles")]
    [SerializeField] private GameObject attack1ParticlePrefab;

    [Header("Player Power Position")]
    [SerializeField] private Transform attackOnePoint;
    [SerializeField] private Transform attackTwoPoint;

    [Header("Player Power Skin Effect")]
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private bool isMage;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        aimCheckCollision = GetComponentInChildren<PlayerAimCheckCollision>();
        shotSystem = GetComponent<ShotSystem>();
        healthSystem = GetComponent<HealthSystem>();

        SetShieldCollisor(false);
        cinemachineVirtualCamera.LookAt = defaultTarget;

        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
    }

    private void Update() 
    {
        Move();
        ApplyGravity();
        Rotate();
        Animations();
        ResetTarget();
    }

    void Move()
    {   
        Vector3 movement = transform.right * inputMove.x + transform.forward * inputMove.y;
        character.Move(movement * stepSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        if(!isLookAtTarget && inputMove.y > 0.7f)
        {
            RotateToForwardCam(turnSpeed);
        }
        else if(target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Set(direction.x, 0f, direction.z);
            transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void RotateToForwardCam(float rotSpeed)
    {
        float yawCam = mainCamera.transform.eulerAngles.y;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, yawCam, 0), rotSpeed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if(!character.isGrounded)
        {
            character.Move(new Vector3(0, gravity, 0));
        }
    }

    void ActivePower()
    {
        if(!isMage)
        {
            isMage = true;
            playerMesh.material.EnableKeyword("_EMISSION");
            fireEffectParticle.Play();
            fireSparklesParticle.Play();
        }
    }

    void Animations()
    {
        isWalking = inputMove.magnitude > 0.1f;
        playerAnimator.SetBool("isWalk", isWalking);

        if(isWalking && animSmoothIncrement < 1)
        {
            playerAnimator.SetFloat("velocity", animSmoothIncrement += (Time.deltaTime * 3));
        }
        else if(animSmoothIncrement > 0)
        {
            playerAnimator.SetFloat("velocity", animSmoothIncrement -= (Time.deltaTime * 3));
        }
    }

    #region CameraTarget
    void SetTarget()
    {   
        if(target == null)
        {
            RotateToForwardCam(15f);

            if(aimCheckCollision.target != null)
            {
                isLookAtTarget = true;
                target = aimCheckCollision.target;
                cinemachineVirtualCamera.gameObject.SetActive(true);
                cinemachineVirtualCamera.LookAt = target;
                UIController.Instance.SetTargetHUD();
            }
        }
        else
        {
           DisableTarget();
        }
    }

    void DisableTarget()
    {
        isLookAtTarget = false;
        target = null;
        UIController.Instance.SetTargetHUD();
        cinemachineVirtualCamera.gameObject.SetActive(false);
        cinemachineVirtualCamera.LookAt = defaultTarget;
    }

    void ResetTarget()
    {
        if(aimCheckCollision.target == null && cinemachineVirtualCamera.LookAt != defaultTarget)
        {
            DisableTarget();
        }
    }

    #endregion

    #region Attack
    void Attack1()
    {
        if(!isAttacking && !isDefending)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("Attack1");

            if(isMage)
            {
                fireEffectParticle.Play();
                if(target != null)
                {
                    StartCoroutine(InstantiateAttackDelay(target.position));
                }
                else
                {
                    StartCoroutine(InstantiateAttackDelay(attackOnePoint.position));
                }
                
            }
        }
    }

    IEnumerator InstantiateAttackDelay(Vector3 position)
    {
        yield return new WaitForSeconds(delayAttackOneSpearEffect);
        attack1SpearParticle.Play();
        yield return new WaitForSeconds(delayToAttackOne);
        Instantiate(attack1ParticlePrefab, position, Quaternion.identity);
    }

    void Attack2()
    {
        if(!isAttacking && !isDefending)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("Attack2");

            if(isMage)
            {
                fireEffectParticle.Play();
                shotSystem.Fire(attackTwoPoint);
            }
        }
    }

    public void FinishAttack() //called by animator on exit animation
    {
        isAttacking = false;
    }

    #endregion

    void Defend(bool def)
    {
        isDefending = def;
        playerAnimator.SetBool("isDefend", isDefending);
    }

    public void SetShieldCollisor(bool enabled) //called by animator
    {
        shieldCollider.enabled = enabled;
    }

    void Interact()
    {

    }

    public void GetHit(int damage)
    {
        healthSystem.Damage(damage);
    }

    public void Death()
    {
        playerAnimator.SetTrigger("Death");
    }

    #region NEW INPUT SYSTEM

    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMove = value.ReadValue<Vector2>().normalized;
    }

    public void OnAttack1(InputAction.CallbackContext value)
    {
        if(value.started) { Attack1(); }
    }

    public void OnAttack2(InputAction.CallbackContext value)
    {
        if(value.started) { Attack2(); ActivePower(); }
    }

    public void OnFocus(InputAction.CallbackContext value)
    {
        if(value.started) { SetTarget(); }
    }

    public void OnDefend(InputAction.CallbackContext value)
    {
        if(value.started) { Defend(true); }
        else if(value.canceled) { Defend(false); }
    }

    public void OnInventory(InputAction.CallbackContext value)
    {
        if(value.started) { }
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if(value.started){ Interact(); }
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if(value.started) { }
    }

    #endregion
}
