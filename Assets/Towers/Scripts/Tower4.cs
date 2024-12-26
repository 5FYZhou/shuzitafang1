using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower4 : MonoBehaviour
{
    [SerializeField]
    private GameObject attackRangePrefab;
    private GameObject towerRange;
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
    //·¶Î§
    [SerializeField]
    private float StrengthenRange;
    [SerializeField]
    private float StrengthenPercentage;

    private List<GameObject> StrengthenTowers = new List<GameObject>();

    private void Awake()
    {
        CreatChild();
    }

    private void Start()
    {
        //CreatChild();
        CreatButton();
    }

    private void GiveAttackRange()
    {
        Vector2 range = new Vector2(StrengthenRange, StrengthenRange);
        if (towerRange)
        {
            towerRange.GetComponent<TowerAttackRange>().SetAttackRange(range);
        }
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
        Transform AttackRange = transform.Find("TowerAttakRange");
        if (!AttackRange)
        {
            towerRange = Instantiate(attackRangePrefab, this.transform.position, Quaternion.identity);
            towerRange.transform.SetParent(this.transform);
            GiveAttackRange();
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

    private void Remove(GameObject Detower)
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tower"))
        {
            IStrengthenTowerAttackPower power = other.GetComponent<IStrengthenTowerAttackPower>();
            if(power != null)
            {
                power.Strengthen(StrengthenPercentage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("tower"))
        {
            IStrengthenTowerAttackPower power = other.GetComponent<IStrengthenTowerAttackPower>();
            if (power != null)
            {
                power.Reduce(StrengthenPercentage);
            }
        }
    }
}
