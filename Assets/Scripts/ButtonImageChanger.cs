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
        if(GetComponent<Image>().sprite == clickedSprite)
            GetComponent<Image>().sprite = defaultSprite;
        else
            GetComponent<Image>().sprite = clickedSprite;
    }
    private void Start()
    {
        GetComponent<Image>().sprite = defaultSprite;
    }
}
