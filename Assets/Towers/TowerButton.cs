using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject aTowerPre;
    public GameObject ATowerPre { get => aTowerPre; }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get => sprite; }

    [SerializeField]
    private float perchasePrice;
    public float PerchasePrice { get => perchasePrice; }

    private Image image;
    private TMP_Text PriceTxt;
    private Button btn;

    private CurrencyManager currencyManager;

    private void Start()
    {
        image = GetComponent<Image>();
        PriceTxt = GetComponentInChildren<TMP_Text>();
        PriceTxt.text = perchasePrice.ToString();
        currencyManager = GameObject.Find("CurrencyCanvas").GetComponent<CurrencyManager>();
        btn = GetComponent<Button>();
    }

    private void Update()
    {
        if (currencyManager.Currency < perchasePrice)
        {
            Grey();
        }
        else
        {
            White();
        }
    }

    public void Grey()
    {
        image.color = Color.gray;
        btn.enabled = false;

    }
    public void White()
    {
        image.color = Color.white;
        btn.enabled = true;
    }
}
