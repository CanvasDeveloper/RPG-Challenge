using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputs : MonoBehaviour
{
    private Vector2 inputMove;

    public Vector2 Movement(InputAction.CallbackContext value)
    {
        return value.ReadValue<Vector2>();
    }

    public bool Attack1(InputAction.CallbackContext value)
    {
        bool returnValue = false;
        if(value.started) { returnValue = true; }
        if(value.canceled) { returnValue = false; }
        return returnValue;
    }

    public bool Attack2(InputAction.CallbackContext value)
    {
        bool returnValue = false;
        if(value.started) { returnValue = true; }
        if(value.canceled) { returnValue = false; }
        return returnValue;
    }

    public bool Defend(InputAction.CallbackContext value)
    {
        bool returnValue = false;
        if(value.started) { returnValue = true; }
        if(value.canceled) { returnValue = false; }
        return returnValue;
    }

    public bool Interact(InputAction.CallbackContext value)
    {
        print(value.ReadValueAsButton());

        bool returnValue = false;
        if(value.started) { returnValue = true; }
        if(value.canceled) { returnValue = false; }
        return returnValue;
    }

    public bool Pause(InputAction.CallbackContext value)
    {
        bool returnValue = false;
        if(value.started) { returnValue = true; }
        if(value.canceled) { returnValue = false; }
        return returnValue;
    }
}
