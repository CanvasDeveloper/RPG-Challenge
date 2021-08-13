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
    [SerializeField]private Image imageHpBar;
    [SerializeField]private GameObject pausePanel;
    [SerializeField]private GameObject optionsPanel;
    [SerializeField]private GameObject gameOverPanel;
    [SerializeField]private Button primaryPauseButton;
    [SerializeField]private Button primaryGameoverButton;

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
        optionsPanel.SetActive(false);
        SetSelectedButton(primaryPauseButton);
        GameController.Instance.ChangeGameState(GameState.PAUSE);
    }

    public void UpdateHpBar(float currentHealth, float maxHealth)
    {
        imageHpBar.fillAmount = currentHealth / maxHealth;
    }

    public void SetSelectedButton(Button button)
    {
        button.Select();
    }

    public bool isPause()
    {
        return pausePanel.activeSelf;
    }

    public void OpenGameOver()
    {
        gameOverPanel.SetActive(true);
        primaryGameoverButton.Select();
    }

    public void QuitToTitle()
    {
        gameOverPanel.SetActive(false);
        FadeController.Instance.ChangeScene("title");
    }

    public void TryAgain()
    {
        FadeController.Instance.ReloadScene();
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
         optionsPanel.SetActive(false);
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
    }
}
