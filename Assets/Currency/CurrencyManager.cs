using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    private float currency;
    [SerializeField]
    private float maxCurrency;

    [SerializeField]
    private TMP_Text currencyTxt;

    public float Currency 
    { 
        get => currency; 
        set 
        { 
            currency = value;
            this.currencyTxt.text = value.ToString();
        } 
    }

    private void Awake()
    {
        //currencyTxt = GetComponent<Text>();
        Currency = currency;
    }

    private void Update()
    {
        if (Currency > maxCurrency)
        {
            Currency = maxCurrency;
        }
    }
}
