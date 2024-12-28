using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    private float currency;

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

    private void Start()
    {
        //currencyTxt = GetComponent<Text>();
        Currency = 50;
    }
}
