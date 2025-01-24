using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower0 : Tower
{
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private GameObject sellButtonPrefab;

    private float Timer = 0;
    [SerializeField]
    private float Cooldown;
    [SerializeField]
    private float Amount;
    private bool Ready = false;

    /*[SerializeField]
    private LayerMask cannotMoveLayer
    {
        get { return CannotMoveLayer; }
        set { CannotMoveLayer = value; }
    }*/
    //血量
    [SerializeField]
    private float health;
    //价格
    [SerializeField]
    private float purchasePrice;
    [SerializeField]
    private float sellingPrice;

    private void Awake()
    {
        InitialHealthVolume = health;
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;

        animator = GetComponent<Animator>();
        CreatHealthBar(healthBarPrefab, 1f, 0.8f);
    }

    private void Start()
    {
        //CreatChild();
        CreatButton(sellButtonPrefab, 0.7f, 0.8f);//不放在Awake里因为等号生成塔时设置图层的时差
    }

    void Update()
    {
        Produce();
    }

    private void Produce()
    {
        if (!Ready)
        {
            Timer += Time.deltaTime;
            if (Timer >= Cooldown)
            {
                Ready = true;
                Timer = 0;
            }
        }
        else
        {
            //Debug.Log("produce");

            GameObject currency = GameObject.Find("CurrencyCanvas");
            currency.GetComponent<CurrencyManager>().Currency += Amount;

            Ready = false;

            animator.SetTrigger("Produce");
        }
    }
}
