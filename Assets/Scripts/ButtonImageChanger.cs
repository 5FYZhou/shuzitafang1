using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChanger : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite clickedSprite;

    public void OnButtonClick()
    {
        if (GetComponent<Image>().sprite == clickedSprite)
        {
            GetComponent<Image>().sprite = defaultSprite;
            GameStateManager.currentGameState = GameState.Playing;
            Time.timeScale = 1f;
        }
        else
        {
            GetComponent<Image>().sprite = clickedSprite;
            GameStateManager.currentGameState = GameState.Paused;
            Time.timeScale = 0;
        }
    }
    private void Start()
    {
        GetComponent<Image>().sprite = defaultSprite;
    }
}
