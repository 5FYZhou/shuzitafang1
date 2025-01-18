using System.Collections.Generic;
using UnityEngine;

public class Tower1 : Tower
{
    /*[SerializeField]
    private string projectileType;*/
    //
    [SerializeField]
    private GameObject attackRangePrefab;
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private GameObject sellButtonPrefab;

    //攻击目标
    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
    }

    private Queue<GameObject> monsters = new();
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
    private float attackPower;
    public float Damage
    {
        get { return AttackPower; }
    }
    //血量
    [SerializeField]
    private float initialHealthVolume;
    //价格
    [SerializeField]
    private float purchasePrice;
    [SerializeField]
    private float sellingPrice;

    private void Awake()
    {
        AttackPower = attackPower;
        InitialHealthVolume = initialHealthVolume;
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;
        Range = AttackRange;

        CreatHealthBar(healthBarPrefab);
        CreatRange(attackRangePrefab);
    }

    void Start()
    {
        //CreatChild();
        CreatButton(sellButtonPrefab);
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Attack();
        //Debug.Log(monsters.Count);
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

                animator.SetTrigger("Attack");
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
                Queue<GameObject> tempQueue = new();
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
}
