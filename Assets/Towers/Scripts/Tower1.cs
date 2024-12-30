using System.Collections.Generic;
using UnityEngine;

public class Tower1 : MonoBehaviour, IStrengthenTowerAttackPower
{
    /*[SerializeField]
    private string projectileType;*/
    //
    [SerializeField]
    private GameObject attackRangePrefab;
    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject towerRange;
    private GameObject bar;
    [SerializeField]
    private GameObject sellButtonPrefab;
    private GameObject button;

    //攻击目标
    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
    }

    private Queue<GameObject> monsters = new Queue<GameObject>();
    //攻击
    private bool canAttack = true;
    public GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }
    //攻击间隔、范围
    private float AttackTimer = 0;
    [SerializeField]
    private float AttackCooldown;
    [SerializeField]
    private float AttackRange;
    
    //攻击力
    [SerializeField]
    private float AttackPower;
    public float Damage
    {
        get { return AttackPower; }
    }

    //血量
    [SerializeField]
    private float initialHealthVolume;
    public float InitialHealthVolume
    {
        get { return initialHealthVolume; }
    }
    //价格
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

    //动画
    private Animator Tower1Animator;

    private void Awake()
    {
        CreatChild();
        GetComponent<Tower>().Price = purchasePrice;
    }

    void Start()
    {
        //CreatChild();
        CreatButton();
        Tower1Animator = GetComponent<Animator>();
    }
    void Update()
    {
        Attack();
        //Debug.Log(monsters.Count);
    }

    private void GiveAttackRange()
    {
        Vector2 range = new Vector2(AttackRange, AttackRange);
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
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            bar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            bar.transform.SetParent(this.transform);
            Vector2 scale = bar.transform.localScale;
            scale = scale * 0.8f;
            bar.transform.localScale = scale;
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

    private void Attack()
    {
        if (!canAttack)
        {
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackCooldown)
            {
                canAttack = true;
                AttackTimer = 0;
            }
        }

        if (target == null && monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }
        if (target != null && !target.GetComponent<enemy>().Death/*怪物活着*/)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;

                Tower1Animator.SetTrigger("Attack");
            }
        }
    }

    private void Shoot()
    {
        //Projectile projectile = /*GameManager.Instance.Pool.GetObject(projectileType).没写*/GetComponent<Projectile>();
        GameObject projectile = Instantiate(this.projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.position = transform.position;

        projectile.GetComponent<Projectile>().Initialize(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            monsters.Enqueue(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            if (target != null && other.gameObject != target.gameObject)
            {
                Queue<GameObject> tempQueue = new Queue<GameObject>();
                while (monsters.Count > 0)
                {
                    GameObject monster0 = monsters.Dequeue();
                    if (monster0.gameObject != other.gameObject)
                    {
                        tempQueue.Enqueue(monster0);
                    }
                }
                monsters.Clear();
                while (tempQueue.Count > 0)
                {
                    monsters.Enqueue(tempQueue.Dequeue());
                }
            }
            else
                target = null;
        }
    }

    public void Strengthen(float per)
    {
        AttackPower = AttackPower * (per + 1);
    }
    public void Reduce(float per)
    {
        AttackPower = AttackPower / (per + 1);
    }
}
