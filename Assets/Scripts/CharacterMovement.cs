using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController character;
    private Animator characterAnimator;

    private Vector2 inputMove;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }

    private void Update() 
    {

    }

    #region NEW INPUT SYSTEM

    

    #endregion
}
