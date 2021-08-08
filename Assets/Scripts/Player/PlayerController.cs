using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    private CharacterController character;
    private Animator playerAnimator;

    [Header("Player Settings")]
    [SerializeField]private BoxCollider shieldCollider;
    [SerializeField]private float gravity = -9.81f;
    [SerializeField]private float stepSpeed;
    [SerializeField]private float turnSpeed = 15f;
    private float animSmoothIncrement = 0;
    private Vector3 inputMove;
    private bool havePowers;

    [Header("Player Aim")]
    [SerializeField]private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField]private Transform defaultTarget;
    public Transform target;
    private bool isLookAtTarget;
    private bool isWalking;
    private bool isAttacking;
    private bool isDefending;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        shieldCollider.enabled = false;
        cinemachineVirtualCamera.LookAt = defaultTarget;

        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
    }

    private void Update() 
    {
        Move();
        ApplyGravity();
        SetTarget();
        Rotate();
        Animations();
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
            float yawCam = mainCamera.transform.eulerAngles.y;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, yawCam, 0), turnSpeed * Time.deltaTime);
        }
        else if(target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Set(direction.x, 0f, direction.z);
            transform.localRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void ApplyGravity()
    {
        if(!character.isGrounded)
        {
            character.Move(new Vector3(0, gravity, 0));
        }
    }

    void SetTarget()
    {
        if(target != null && cinemachineVirtualCamera.LookAt != target)
        {
            isLookAtTarget = true;
            cinemachineVirtualCamera.gameObject.SetActive(true);
            cinemachineVirtualCamera.LookAt = target;
        }
        else if(target == null && cinemachineVirtualCamera.LookAt != defaultTarget)
        {
            isLookAtTarget = false;
            cinemachineVirtualCamera.gameObject.SetActive(false);
            cinemachineVirtualCamera.LookAt = defaultTarget;
        }
    }

    void Animations()
    {
        isWalking = inputMove.magnitude > 0.1f;
        playerAnimator.SetBool("isWalk", isWalking);

        if(isWalking && animSmoothIncrement < 1)
        {
            playerAnimator.SetFloat("velocity", animSmoothIncrement += Time.deltaTime);
        }
        else if(animSmoothIncrement > 0)
        {
            playerAnimator.SetFloat("velocity", animSmoothIncrement -= Time.deltaTime);
        }
    }

    #region Attack
    void Attack1()
    {
        if(!isAttacking && !isDefending)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("Attack1");
        }
    }

    void Attack2()
    {
        if(!isAttacking && !isDefending)
        {
            isAttacking = true;
            playerAnimator.SetTrigger("Attack2");
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
        shieldCollider.enabled = isDefending;
    }

    void Interact()
    {

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
        if(value.started) { Attack2(); }
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
