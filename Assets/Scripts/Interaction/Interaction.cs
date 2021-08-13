using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    void Interact();
}

public class Interaction : MonoBehaviour
{
    [SerializeField]private GameObject interactionText;

    public void StartInteraction()
    {
        transform.parent.gameObject.SendMessage("Interact", SendMessageOptions.RequireReceiver);
    }

    public void SetInteractionText(bool active)
    {
        interactionText.SetActive(active);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            SetInteractionText(true);
        }   
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            SetInteractionText(false);
        }   
    }
}
