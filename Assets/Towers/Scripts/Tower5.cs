using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower5 : Tower
{
    [SerializeField]
    private GameObject attackRangePrefab;
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private GameObject sellButtonPrefab;

    //����Ŀ��
    [SerializeField]
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
    }

    private Queue<GameObject> monsters = new();
    //����
    private bool canAttack = true;
    public GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }
    //�����������Χ
    private float AttackTimer = 0;
    [SerializeField]
    private float AttackCooldown;
    [SerializeField]
    private float AttackRange;

    //������
    [SerializeField]
    private float attackPower;
    [SerializeField]
    private float SplashAttackPower;
    public float Damage
    {
        get { return AttackPower; }
    }
    public float SplashDamage 
    {
        get { return SplashAttackPower; }
    }

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
        CreatButton(sellButtonPrefab);
        //animator = GetComponent<Animator>();

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
        if (target != null && !target.GetComponent<enemy>().Death/*�������*/)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;

                //animator.SetTrigger("Attack");
            }
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(this.projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.position = transform.position;

        projectile.GetComponent<T5Projectile>().Initialize(this);
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
            {
                target = null;
            }
        }
    }
}
