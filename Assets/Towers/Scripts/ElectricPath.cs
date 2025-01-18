using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ElectricPath : MonoBehaviour
{
    private SpriteRenderer tileRenderer;
    private BoxCollider2D boxCollider;

    public Tower2 towerA;
    public Tower2 towerB;
    private Tower2Manager tower2Manager;
    private Vector3 PrePosA;
    private Vector3 PrePosB;

    private bool canAttack = true;
    private float AttackTimer = 0;
    private float AttackCooldown;
    [SerializeField]
    private float Damage;
    private List<GameObject> targets = new();

    //private float damageA;
    //private float damageB;

    private Animator towerAanimator;
    private Animator towerBanimator;

    private void Start()
    {
        tileRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CheckPositionChanges();
        Attack();
        ChangeDamage();
        //Debug.Log(targets.Count);
    }

    public void SetTowers(Tower2 A, Tower2 B, Tower2Manager Manager)
    {
        towerA = A;
        towerB = B;
        PrePosA = towerA.transform.position;
        PrePosB = towerB.transform.position;
        tower2Manager = Manager;
        AttackCooldown = towerA.AttackCooldown;
        Damage = towerA.Damage > towerB.Damage ? towerA.Damage : towerB.Damage;
        //damageA = towerA.Damage;
        //damageB = towerB.Damage;
        towerAanimator = towerA.Myanimator;
        towerBanimator = towerB.Myanimator;
    }

    private void Remove(GameObject Detarget)
    {
        List<GameObject> Newtargets = new();
        foreach (GameObject target in targets)
        {
            if (target != Detarget)
            {
                Newtargets.Add(target);
            }
        }
        targets = Newtargets;
    }

    private void CheckPositionChanges()
    {
        if (towerA && towerB && (towerA.GetComponent<Tower2>().CanAlignedWith(towerB) || towerB.GetComponent<Tower2>().CanAlignedWith(towerA)) && (!Vector3.Equals(PrePosA, towerA.transform.position) || !Vector3.Equals(PrePosB, towerB.transform.position)))
        {
            //if (!Vector3.Equals(PrePosA, towerA.transform.position) || !Vector3.Equals(PrePosB, towerB.transform.position))
            //{
                //Debug.Log($"ChangePosition{towerA}{towerB}");
                PrePosA = towerA.transform.position;
                PrePosB = towerB.transform.position;
                transform.position = (PrePosA + PrePosB) / 2;
            /*Vector2 scale = transform.localScale;
            scale.x = Mathf.Abs(PrePosA.x - PrePosB.x) + Mathf.Abs(PrePosA.y - PrePosB.y) - 1f;
            transform.localScale = scale;*/
            float width = Mathf.Abs(PrePosA.x - PrePosB.x) + Mathf.Abs(PrePosA.y - PrePosB.y) - 0.5f;
            tileRenderer.size = new Vector2(width, 0.1f);
            boxCollider.size = new Vector2(width, 0.2f);
            // }
        }
        
        else if(towerA == null || towerB == null)
        {
            tower2Manager.DestroyElectricPath(this);
        }
        /*
        else if(!towerA.GetComponent<Tower2>().CanAlignedWith(towerB))
        {
//            Debug.Log($"Destroy{towerA}{towerB}");
            Destroy(gameObject);
            Remove(tower2Manager, this);
            //towerA.Delect(towerA.electricPaths, gameObject);
            //towerB.Delect(towerB.electricPaths, gameObject);
        }*/
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
        if (targets.Count > 0 && canAttack)
        {
            //Debug.Log("2sttack");
            foreach(GameObject target in targets)
            {
                target.GetComponent<enemy>().attack(Damage);//对怪物造成伤害
                canAttack = false;

                towerAanimator.SetTrigger("Attack");
                towerBanimator.SetTrigger("Attack");
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            Remove(other.gameObject);
        }
    }

    public void ChangeDamage()
    {
        /*if (towerA.Damage > damageA)
        {
            Damage = towerA.Damage;
            damageA = towerA.Damage;
        }
        if (towerB.Damage > damageB)
        {
            Damage = towerB.Damage;
            damageB = towerB.Damage;
        }*/
        float damage = towerA.Damage > towerB.Damage ? towerA.Damage : towerB.Damage;
        if (damage != Damage)
        {
            Damage = damage;
            //Debug.Log(Damage);
        }
    }
}
