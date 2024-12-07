using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower3 : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject bar;

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

    // Start is called before the first frame update
    void Start()
    {
        CreatChild();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.8f, 0f);
            bar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            bar.transform.SetParent(this.transform);
            GiveHealthVolumn();
        }
    }
}
