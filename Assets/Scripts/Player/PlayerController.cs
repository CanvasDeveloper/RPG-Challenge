using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public enum PlayerState
{
    ALIVE, DEAD
}

[RequireComponent(typeof(ShotSystem))]
[RequireComponent(typeof(HealthSystem))]
public class PlayerController : MonoBehaviour, IHealthSystem
{
    private Camera mainCamera;
    private CharacterController character;
    private Animator playerAnimator;
    private RayCastCheckTarget rayCastCheckTarget;
    private ShotSystem shotSystem;
    private HealthSystem healthSystem;

    [Header("Player Settings")]
    public PlayerState currentState;
    [SerializeField]private BoxCollider shieldCollider;
    [SerializeField]private BoxCollider normalAttackCollider;
    [SerializeField]private BoxCollider normalAttackCollider1;
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
    private EnemyStateController enemyTargetState;
    private bool isLookAtTarget;
    private bool isWalking;
    private bool isAttacking;
    private bool isDefending;
    private bool isVictory;
    private bool isInteract;

    [Header("Player Particles")]
    [SerializeField] private ParticleSystem fireEffectParticle;
    [SerializeField] private ParticleSystem fireSparklesParticle;
    [SerializeField] private ParticleSystem attack1SpearParticle;
    [SerializeField] private ParticleSystem healthParticle;

    [Header("Player Spawn Particles")]
    [SerializeField] private GameObject attack1ParticlePrefab;

    [Header("Player Power Position")]
    [SerializeField] private Transform attackOnePoint;
    [SerializeField] private Transform attackTwoPoint;

    [Header("Player Power Skin Effect")]
    [SerializeField] private SkinnedMeshRenderer playerMesh;
    [SerializeField] private bool isMage;

    [Header("Interaction")]
    public Interaction interactionObject;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        rayCastCheckTarget = GetComponentInChildren<RayCastCheckTarget>();
        shotSystem = GetComponent<ShotSystem>();
        healthSystem = GetComponent<HealthSystem>();

        SetShieldCollisor(false);
        normalAttackCollider.enabled = false;
        normalAttackCollider1.enabled = false;
        mainCamera = Camera.main;
        cinemachineVirtualCamera.LookAt = defaultTarget;

        if(Inventory.Instance.CheckIfHaveMage())
        {
            ActivePower();
        }
    }

    private void Update() 
    {
        ApplyGravity();
        if(currentState != PlayerState.DEAD)
        {
            Move();
            Rotate();
            MoveAnimation();
            ResetTarget();
            CheckVictory();
            CheckInteract();
        }
    }

    void CheckVictory()
    {
        if(GameController.Instance.currentState == GameState.VICTORY && !isVictory)
        {
            isVictory = true;
            playerAnimator.SetTrigger("Victory");
        }
    }

    void CheckInteract()
    {
        if(isInteract && GameController.Instance.currentState == GameState.GAMEPLAY)
        {
            isInteract = false;
        }
        else if(!isInteract && GameController.Instance.currentState == GameState.DIALOG)
        {
            isInteract = true;
            inputMove = Vector3.zero;
            Move();
        }
    }

    void Move()
    {   
        if(!isLookAtTarget)
        {
            Vector3 movement = transform.forward * inputMove.magnitude;
            character.Move(movement * stepSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 movement = transform.right * inputMove.x + transform.forward * inputMove.y;
            character.Move(movement * stepSpeed * Time.deltaTime); //carangueja sksks
        }
    }

    void Rotate()
    {
        if(!isLookAtTarget && inputMove.magnitude != 0)
        {
            float targetAngle = Mathf.Atan2(inputMove.x, inputMove.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
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
            character.Move(new Vector3(0, Physics.gravity.y * Time.fixedDeltaTime, 0));
        }
    }

    public void ActivePower()
    {
        if(!isMage)
        {
            isMage = true;
            playerMesh.material.EnableKeyword("_EMISSION");
            fireEffectParticle.Play();
            fireSparklesParticle.Play();
        }
    }

    void MoveAnimation()
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

            if(rayCastCheckTarget.target != null)
            {
                isLookAtTarget = true;
                target = rayCastCheckTarget.target;
                enemyTargetState = target.GetComponent<EnemyStateController>();
                cinemachineVirtualCamera.gameObject.SetActive(true);
                cinemachineVirtualCamera.LookAt = target.transform;
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
        enemyTargetState = null;
        UIController.Instance.SetTargetHUD();
        cinemachineVirtualCamera.gameObject.SetActive(false);
        cinemachineVirtualCamera.LookAt = defaultTarget;
    }

    void ResetTarget()
    {
        if(rayCastCheckTarget.target == null && cinemachineVirtualCamera.LookAt != defaultTarget)
        {
            DisableTarget();
        }
        else if(enemyTargetState != null && enemyTargetState.currentState == EnemyState.DEAD)
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
                    StartCoroutine(InstantiateAttackDelay(target.transform.position));
                }
                else
                {
                    StartCoroutine(InstantiateAttackDelay(attackOnePoint.position));
                }
            }
            else
            {
                StartCoroutine(NormalAttack1());
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
            else
            {
                StartCoroutine(NormalAttack());
            }
        }
    }

    IEnumerator NormalAttack()
    {
        yield return new WaitForSeconds(0.25f);
        normalAttackCollider.enabled = true;
        yield return new WaitForSeconds(0.15f);
        normalAttackCollider.enabled = false;
    }

    IEnumerator NormalAttack1()
    {
        yield return new WaitForSeconds(0.25f);
        normalAttackCollider1.enabled = true;
        yield return new WaitForSeconds(0.15f);
        normalAttackCollider1.enabled = false;
    }

    public void FinishAttack() //called by animator on exit animation
    {
        isAttacking = false;
    }

    #endregion

    void Defend()
    {
        isDefending = true;
        playerAnimator.SetBool("isDefend", isDefending);
    }

    void ReleaseDefend()
    {
        isDefending = false;
        playerAnimator.SetBool("isDefend", isDefending);
        SetShieldCollisor(false);
    }   

    public void SetShieldCollisor(bool enabled) //called by animator
    {
        shieldCollider.enabled = enabled;
    }

    void UseItem()
    {
        if(Inventory.Instance.equippedItem != null)
        {
            healthSystem.RecoveryHealth(Inventory.Instance.equippedItem.healthRecovery);
            Inventory.Instance.SubtractValue();
            healthParticle.Play();
            UIController.Instance.UpdateHpBar(healthSystem.currentHealth , healthSystem.maxHealth);
        }
    }

    void Interact()
    {
        if(interactionObject != null)
        {
            interactionObject.StartInteraction();
        }
    }

    public void GetHit(int damage)
    {
        playerAnimator.SetTrigger("GetHit");
        healthSystem.Damage(damage);
        UIController.Instance.UpdateHpBar(healthSystem.currentHealth, healthSystem.maxHealth);
    }

    public void Death()
    {
        playerAnimator.SetTrigger("Death");
        ChangeState(PlayerState.DEAD);
    }

    void ChangeState(PlayerState newState)
    {
        currentState = newState;
        switch(currentState)
        {
            case PlayerState.DEAD:
                character.detectCollisions = false;
                GameController.Instance.ChangeGameState(GameState.GAMEOVER);
            break;
        }
    }

    #region NEW INPUT SYSTEM

    public void OnMovement(InputAction.CallbackContext value)
    {
        if(GameController.Instance.currentState == GameState.GAMEPLAY && !isInteract)
        {
            inputMove = value.ReadValue<Vector2>().normalized;
        }
    }

    public void OnAttack1(InputAction.CallbackContext value)
    {
        if(GameController.Instance.currentState == GameState.PAUSE) { return; }
    
        if(value.started && currentState != PlayerState.DEAD) { Attack1(); }
    }

    public void OnAttack2(InputAction.CallbackContext value)
    {
        if(GameController.Instance.currentState == GameState.PAUSE) { return; }

        if(value.started && currentState != PlayerState.DEAD) { Attack2(); }
    }

    public void OnFocus(InputAction.CallbackContext value)
    {
        if(value.started && currentState != PlayerState.DEAD) { SetTarget(); }
    }

    public void OnDefend(InputAction.CallbackContext value)
    {
        if(value.started && currentState != PlayerState.DEAD) { Defend(); }
        if(value.canceled && currentState != PlayerState.DEAD) { ReleaseDefend(); }
    }

    public void OnInventory(InputAction.CallbackContext value)
    {
        if(value.started && currentState != PlayerState.DEAD)
        {
            if(Inventory.Instance.isOpen()) { Inventory.Instance.Close(); }
            else if(!UIController.Instance.isPause()){ Inventory.Instance.Open(); }
        }
    }

    public void OnUseItem(InputAction.CallbackContext value)
    {
        if(GameController.Instance.currentState == GameState.DIALOG) { return; }
        if(value.started) { UseItem(); }
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if(value.started && currentState != PlayerState.DEAD){ Interact(); }
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        if(value.started && currentState != PlayerState.DEAD)
        { 
            if(UIController.Instance.isPause()) {UIController.Instance.ClosePause(); }
            else if(!Inventory.Instance.isOpen()){ UIController.Instance.OpenPause();} 
        }
    }

    #endregion
}
