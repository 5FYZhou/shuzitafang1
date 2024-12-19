using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower3 : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject bar;
    [SerializeField]
    private GameObject sellButtonPrefab;
    private GameObject button;

    //ÑªÁ¿
    [SerializeField]
    private float initialHealthVolume;
    public float InitialHealthVolume
    {
        get { return initialHealthVolume; }
    }
    //¼Û¸ñ
    [SerializeField]
    private float purchasePrice;
    public float PurchasePrice
    {
        get { return purchasePrice; }
    }
    [SerializeField]
    private float sellingPrice;
    public float SellingPrice
    {
        get { return sellingPrice; }
    }

    private void Awake()
    {
        CreatChild();
    }

    void Start()
    {
        //CreatChild();
        CreatButton();
    }

    private void GiveHealthVolumn()
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(InitialHealthVolume);
        }
    }

    private void GiveSellButton()
    {
        SellTower sellTower = GetComponentInChildren<SellTower>();
        if (sellTower != null)
        {
            sellTower.Initialize(sellingPrice, GetComponent<Tower>());
        }
    }

    private void CreatChild()
    {
        Transform HealthBar = transform.Find("HealthBarCanvas");
        if (!HealthBar)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.5f, 0f);
            bar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            bar.transform.SetParent(this.transform);
            GiveHealthVolumn();
        }
    }

    private void CreatButton()
    {
        if (gameObject.layer != 11 && gameObject.layer != 12)
        {
            Transform SellButton = transform.Find("SellButtonCanvas");
            if (!SellButton)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y - 1f, 0f);
                button = Instantiate(sellButtonPrefab, position, Quaternion.identity);
                button.transform.SetParent(this.transform);
                GiveSellButton();
            }
        }
    }
}
