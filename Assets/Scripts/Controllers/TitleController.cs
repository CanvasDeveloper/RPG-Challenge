using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField]private Button primaryButton;

    void Start()
    {
        primaryButton.Select();
    }

    public void SetSelectedButton(Button button)
    {
        button.Select();
    }

    public void Play()
    {
        FadeController.Instance.NextScene();
    }

    public void Exit()
    {
        FadeController.Instance.Exit();
    }
}
