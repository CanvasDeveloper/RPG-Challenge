using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private Camera cam;
    [SerializeField] private Image imgAim;
    [SerializeField]private GameObject pausePanel;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void SetTargetHUD()
    {
        imgAim.enabled = !imgAim.enabled;
    }

    public void OpenPause()
    {
        pausePanel.SetActive(true);
        GameController.Instance.ChangeGameState(GameState.PAUSE);
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
    }
}
