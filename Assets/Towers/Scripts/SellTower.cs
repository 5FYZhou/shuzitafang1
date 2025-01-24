using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellTower : MonoBehaviour
{
    private bool show = false;
    private float timer = 0;
    private Image image;
    private Button button;
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private float sellPrice;

    [SerializeField]
    private float ShowCD;

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
            if (timer >= ShowCD)
            {
                timer = 0;
                show = false;

                image.enabled = false;
                button.enabled = false;
                text.enabled = false;
            }
        }
    }

    public void Initialize(float sellprice)
    {
        sellPrice = sellprice;
        if (text != null)
            text.text = sellprice.ToString();
        //else
            //Debug.Log("A");
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
        Destroy(transform.parent.parent.gameObject);

        GameObject.Find("Grid").GetComponentInChildren<CreatTowerManager>().TowerNumber -= 1;
    }
}
