using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MENU, GAMEPLAY, PAUSE, GAMEOVER, DIALOG, VICTORY
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameState currentState;
    public Collectable cristalFireItem;

    private void Awake() {
        if(Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else{ Destroy(gameObject); }
    }

    public void ChangeGameState(GameState newState)
    {
        currentState = newState;
        switch(newState)
        {
            case GameState.GAMEPLAY:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            break;

            case GameState.PAUSE:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            break;

            case GameState.GAMEOVER:
                StartCoroutine(DelayGameOver());
            break;

            case GameState.DIALOG:

            break;

            case GameState.VICTORY:
                Time.timeScale = 0;
                UIController.Instance.OpenVictoryPanel();
                ChangeGameState(GameState.PAUSE);
            break;
        }
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(3f);
        UIController.Instance.OpenGameOver();
    }
}
