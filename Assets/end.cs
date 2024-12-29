using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end : MonoBehaviour
{
    public void OnButtonClick()
    {
        GameStateManager.currentGameState = GameState.GameOver;
    }
}
