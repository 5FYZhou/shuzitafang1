using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}
public class GameStateManager : MonoBehaviour
{
    public static GameState currentGameState;
}
