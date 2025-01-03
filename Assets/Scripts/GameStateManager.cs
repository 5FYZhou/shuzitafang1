using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}
public class GameStateManager : MonoBehaviour
{
    public static GameState currentGameState;

    private void Update()
    {
        if (currentGameState == GameState.GameOver)
        {
            SceneManager.LoadSceneAsync("MainScene");
        }
        
    }
}
