using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower3 : Tower
{
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private GameObject sellButtonPrefab;

    //Ѫ��
    [SerializeField]
    private float initialHealthVolume;
    //�۸�
    [SerializeField]
    private float purchasePrice;
    [SerializeField]
    private float sellingPrice;

    private void Awake()
    {
        InitialHealthVolume = initialHealthVolume;
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;

        CreatHealthBar(healthBarPrefab, 1f, 0.8f);
    }

    void Start()
    {
        //CreatChild();
        CreatButton(sellButtonPrefab, 0.7f, 0.8f);
    }
}
