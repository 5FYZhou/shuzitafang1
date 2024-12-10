using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower0 : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject bar;

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

    private void Start()
    {
        CreatChild();
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
            Debug.Log("produce");
            Ready = false;

            animator.SetTrigger("Produce");
        }
    }
}
