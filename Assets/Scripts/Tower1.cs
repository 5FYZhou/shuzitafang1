using System.Collections.Generic;
using UnityEngine;

public class Tower1 : MonoBehaviour
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

    //����Ŀ��
    private Monster target;
    public Monster Target
    {
        get { return target; }
    }

    private Queue<Monster> monsters = new Queue<Monster>();
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
    private float AttackPower;
    public float Damage
    {
        get { return AttackPower; }
    }

    //Ѫ��
    [SerializeField]
    private float initialHealthVolume;
    public float InitialHealthVolume
    {
        get { return initialHealthVolume; }
    }
    //�۸�
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

    void Start()
    {
        CreatChild();
    }
    void Update()
    {
        Attack();
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

    private void CreatChild()
    {
        Transform AttackRange = transform.Find("TowerAttakRange");
        if (!AttackRange)
        {
            towerRange = Instantiate(attackRangePrefab, this.transform.position, Quaternion.identity);
            towerRange.transform.SetParent(this.transform);
            GiveAttackRange();
        }
        Transform HealthBar = transform.Find("HealthBarCanvas");
        if (!HealthBar)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            bar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            bar.transform.SetParent(this.transform);
            GiveHealthVolumn();
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
        if (target != null /*&&target.IsActive�������*/)
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
        //Projectile projectile = /*GameManager.Instance.Pool.GetObject(projectileType).ûд*/GetComponent<Projectile>();
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