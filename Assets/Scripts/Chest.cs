using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteraction
{
    public void Interact()
    {
        GameController.Instance.ChangeGameState(GameState.VICTORY);
    }
}
