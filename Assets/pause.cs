using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour
{
    public void OnButtonClick()
    {
        if (GameStateManager.currentGameState == GameState.Playing)
        {
            GameStateManager.currentGameState = GameState.Paused;
        }
        else if (GameStateManager.currentGameState == GameState.Paused)
        {
            GameStateManager.currentGameState = GameState.Playing;
        }
    }
}
