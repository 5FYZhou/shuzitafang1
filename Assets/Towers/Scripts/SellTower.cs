using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellTower : MonoBehaviour
{
    private bool show = true;
    private float timer = 0;
    private Image image;
    private Button button;
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private float sellPrice;

    private Tower parent;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        //image.enabled = false;
    }

    private void Update()
    {
        if (show)
        {
            image.enabled = true;
            button.enabled = true;
            text.enabled = true;

            timer += Time.deltaTime;
            if (timer >= 3f)
            {
                timer = 0;
                show = false;

                image.enabled = false;
                button.enabled = false;
                text.enabled = false;
            }
        }
    }

    public void Initialize(float sellprice,Tower Parent)
    {
        sellPrice = sellprice;
        parent = Parent;
        if (text != null)
            text.text = sellprice.ToString();
        else
            Debug.Log("A");
    }

    public void Show()
    {
        show = true;
        timer = 0;
    }

    public void Sell()
    {
        //Debug.Log("Sell"+sellPrice);
        GameObject currency = GameObject.Find("CurrencyCanvas");
        currency.GetComponent<CurrencyManager>().Currency += sellPrice;
        parent.DestroyTower();

        GameObject.Find("Grid").GetComponentInChildren<CreatTowerManager>().TowerNumber -= 1;
    }
}
