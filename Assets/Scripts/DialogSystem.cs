using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct DialogStruct
{
    public string npcName;
    public string npcText;
    public string animationName;
}

public class DialogSystem : MonoBehaviour
{
    private Animator dialogAnim;
    [SerializeField]private DialogStruct[] dialogs;
    [SerializeField]private DialogStruct finishDialog;
    [SerializeField]private DialogStruct specialDialog;

    private int idDialog = 0;
    public bool isFinishDialog;

    private void Start() {
        dialogAnim = GetComponent<Animator>();
    }

    public void StartDialog()
    {
        GameController.Instance.ChangeGameState(GameState.DIALOG);
        UIController.Instance.OpenDialogPanel(dialogs[0].npcName, dialogs[0].npcText);
        if(dialogs[0].animationName != null) { dialogAnim.SetTrigger(dialogs[0].animationName);}
    }

    public void NextDialog()
    {
        if(!isFinishDialog)
        {
            idDialog++;
            if(idDialog < dialogs.Length)
            {
                UIController.Instance.SetDialogText(dialogs[idDialog].npcName, dialogs[idDialog].npcText);
                if(dialogs[idDialog].animationName != null) { dialogAnim.SetTrigger(dialogs[idDialog].animationName);}
            }
            else
            {
                UIController.Instance.CloseDialogPanel();
                isFinishDialog = true;
            }
        }
        else
        {
            UIController.Instance.OpenDialogPanel(finishDialog.npcName, finishDialog.npcText);
        }
    }

    public void SpecialDialog()
    {
        if(UIController.Instance.CheckIfDialog())
        {
            UIController.Instance.CloseDialogPanel();
        }
        else
        {
            UIController.Instance.OpenDialogPanel(specialDialog.npcName, specialDialog.npcText);
            dialogAnim.SetTrigger(specialDialog.animationName);
        }
    }

    public void FinishDialog()
    {
        if(UIController.Instance.CheckIfDialog())
        {
            UIController.Instance.CloseDialogPanel();
        }
        else
        {
            UIController.Instance.OpenDialogPanel(finishDialog.npcName, finishDialog.npcText);
            dialogAnim.SetTrigger(finishDialog.animationName);
        }
    }
}

