using System.Collections.Generic;
using UnityEngine;

public class Tower1 : MonoBehaviour
{
    /*[SerializeField]
    private string projectileType;*/
    //¹¥»÷Ä¿±ê
    private Monster target;
    public Monster Target
    {
        get { return target; }
    }

    private Queue<Monster> monsters = new Queue<Monster>();
    //¹¥»÷
    private bool canAttack = true;
    public GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
    }
    //¹¥»÷¼ä¸ô¡¢·¶Î§
    private float AttackTimer = 0;
    [SerializeField]
    private float AttackCooldown;
    [SerializeField]
    private float AttackRange;
    
    //¹¥»÷Á¦
    [SerializeField]
    private float AttackPower;
    public float Damage
    {
        get { return AttackPower; }
    }

    void Start()
    {
        GiveAttackRange();
    }
    void Update()
    {
        Attack();
    }

    private void GiveAttackRange()
    {
        Vector2 range = new Vector2(AttackRange, AttackRange);
        TowerAttackRange towerRange = GetComponentInChildren<TowerAttackRange>();
        if (towerRange)
        {
            towerRange.SetAttackRange(range);
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
        if (target != null /*&&target.IsActive¹ÖÎï»î×Å*/)
        {
            if (canAttack)
            {
                Shoot();

                canAttack = false;
            }
        }
    }

    private void Shoot()
    {
        //Projectile projectile = /*GameManager.Instance.Pool.GetObject(projectileType).Ã»Ð´*/GetComponent<Projectile>();
        GameObject projectile = Instantiate(this.projectilePrefab);
        projectile.transform.position = transform.position;

        projectile.GetComponent<Projectile>().Initialize(this);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (target != null && other.gameObject != target.gameObject)
            {
                Queue<Monster> tempQueue = new Queue<Monster>();
                while (monsters.Count > 0)
                {
                    Monster monster0 = monsters.Dequeue();
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
