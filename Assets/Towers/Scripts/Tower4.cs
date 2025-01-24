using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower4 : Tower
{
    [SerializeField]
    private GameObject attackRangePrefab;
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private GameObject sellButtonPrefab;

    //血量
    [SerializeField]
    private float initialHealthVolume;
    //价格
    [SerializeField]
    private float purchasePrice;
    [SerializeField]
    private float sellingPrice;
    //范围
    [SerializeField]
    private float StrengthenRange;
    //百分比
    [SerializeField]
    private float StrengthenPercentage;

    //private List<GameObject> StrengthenTowers = new();

    private void Awake()
    {
        InitialHealthVolume = initialHealthVolume;
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;
        Range = StrengthenRange;

        CreatHealthBar(healthBarPrefab, 1f, 0.8f);
        CreatRange(attackRangePrefab);
    }

    private void Start()
    {
        //CreatChild();
        CreatButton(sellButtonPrefab, 0.7f, 0.8f);
    }

    /*private void Remove(GameObject Detower)
    {
        List<GameObject> NewTowers = new List<GameObject>();
        foreach (GameObject tower in StrengthenTowers)
        {
            if (tower != Detower)
            {
                NewTowers.Add(tower);
            }
        }
        StrengthenTowers = NewTowers;
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tower"))
        {
            Tower power = other.GetComponent<Tower>();
            if(power != null)
            {
                power.StrengthenAttackPower(StrengthenPercentage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("tower"))
        {
            Tower power = other.GetComponent<Tower>();
            if (power != null)
            {
                power.ReduceAttackPower(StrengthenPercentage);
            }
        }
    }
}
