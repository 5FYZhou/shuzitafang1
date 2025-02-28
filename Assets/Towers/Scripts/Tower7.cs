using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower7 : Tower
{
    [SerializeField]
    private GameObject attackRangePrefab;
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private GameObject sellButtonPrefab;

    private Queue<GameObject> monsters = new();
    //¹¥»÷
    private bool canAttack = true;
    //¹¥»÷¼ä¸ô¡¢·¶Î§
    private float AttackTimer = 0;
    [SerializeField]
    private float AttackCooldown;
    [SerializeField]
    private float AttackRange;
    //¹¥»÷Á¦
    [SerializeField]
    private float attackPower;
    private float deceleratePer;
    public float Damage
    {
        get { return AttackPower; }
    }
    //ÑªÁ¿
    [SerializeField]
    private float initialHealthVolume;
    //¼Û¸ñ
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

        CreatHealthBar(healthBarPrefab, 1f, 0.8f);
        CreatRange(attackRangePrefab);
    }

    void Start()
    {
        //CreatChild();
        CreatButton(sellButtonPrefab, 0.7f, 0.8f);
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

        if (monsters.Count > 0 && canAttack)
        {
            Shoot();
            canAttack = false;

            //animator.SetTrigger("Attack");
        }
    }

    private void Shoot()
    {
        foreach (GameObject target in monsters)
        {
            if(target != null && !target.GetComponent<enemy>().Death)
                target.GetComponent<enemy>().attack(AttackPower);
        }
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
    }
}
