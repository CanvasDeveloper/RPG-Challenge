using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAim : MonoBehaviour
{
    private CharacterController character;
    private Animator characterAnimator;

    private Vector2 inputMove;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }
}
