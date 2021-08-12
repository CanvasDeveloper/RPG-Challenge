using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private Camera cam;
    [SerializeField] private Image imgAim;
    [SerializeField]private GameObject takeItemPrefab;
    [SerializeField]private Transform takeItemContainer;
    public GameObject pausePanel;
    [SerializeField]private Button primaryPauseButton;

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

    public void TakeItemHUD(Collectable item)
    {
        TakeItemNotification temp = Instantiate(takeItemPrefab, takeItemContainer).GetComponent<TakeItemNotification>();
        temp.UpdateNotification(item.itemIcon, item.itemName);
    }

    public void SetTargetHUD()
    {
        imgAim.enabled = !imgAim.enabled;
    }

    public void OpenPause()
    {
        pausePanel.SetActive(true);
        SetSelectedButton(primaryPauseButton);
        GameController.Instance.ChangeGameState(GameState.PAUSE);
    }

    public void SetSelectedButton(Button button)
    {
        button.Select();
    }

    public bool isPause()
    {
        return pausePanel.activeSelf;
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
    }
}
