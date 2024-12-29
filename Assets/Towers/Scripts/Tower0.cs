using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower0 : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject bar;
    [SerializeField]
    private GameObject sellButtonPrefab;
    private GameObject button;

    private float Timer = 0;
    [SerializeField]
    private float Cooldown;
    [SerializeField]
    private float Amount;
    private bool Ready = false;

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

    private Animator animator;

    private void Awake()
    {
        CreatChild();
        GetComponent<Tower>().Price = purchasePrice;
    }
    private void Start()
    {
        //CreatChild();
        CreatButton();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Produce();
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
        if(sellTower != null)
        {
            sellTower.Initialize(sellingPrice, GetComponent<Tower>());
        }
    }

    private void CreatChild()
    {
        Transform HealthBar = transform.Find("HealthBarCanvas");
        if (!HealthBar)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            bar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            bar.transform.SetParent(this.transform);
            Vector2 scale = bar.transform.localScale;
            scale = scale * 0.8f;
            bar.transform.localScale = scale;
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
                Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.7f, 0f);
                button = Instantiate(sellButtonPrefab, position, Quaternion.identity);
                button.transform.SetParent(this.transform);
                Vector2 scale = button.transform.localScale;
                scale = scale * 0.8f;
                button.transform.localScale = scale;
                GiveSellButton();
            }
        }
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
