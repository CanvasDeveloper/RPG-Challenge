using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MENU, GAMEPLAY, PAUSE
}

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameState currentState;

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
        }
    }
}
