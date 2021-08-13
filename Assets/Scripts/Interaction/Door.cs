using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteraction
{
    public void Interact()
    {
        FadeController.Instance.NextScene();
    }
}
