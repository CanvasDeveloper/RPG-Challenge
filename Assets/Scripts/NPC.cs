using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteraction
{
    private DialogSystem dialogSystem;
    [SerializeField]private GameObject cristalFire;
    [SerializeField]private GameObject door;
    [SerializeField]private Collectable requiredItem;
    bool isFirstInteraction;

    private void Start() {
        dialogSystem = GetComponent<DialogSystem>();
    }

    public void Interact()
    {
        if(Inventory.Instance.CheckItens(requiredItem))
        {
            dialogSystem.FinishDialog();
            door.SetActive(false);
            cristalFire.SetActive(true);
        }
        else if(!isFirstInteraction)
        {
            isFirstInteraction = true;
            dialogSystem.StartDialog();
        }
        else if(dialogSystem.isFinishDialog)
        {
            dialogSystem.SpecialDialog();
        }
        else
        {
            dialogSystem.NextDialog();
        }
    }
}
