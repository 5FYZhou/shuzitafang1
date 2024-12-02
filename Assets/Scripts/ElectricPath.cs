using System.Collections.Generic;
using UnityEngine;

public class ElectricPath : MonoBehaviour
{
    public Tower2 towerA;
    public Tower2 towerB;
    private Tower2Manager tower2Manager;
    private Vector3 PrePosA;
    private Vector3 PrePosB;

    private bool canAttack = true;
    private float AttackTimer = 0;
    private float AttackCooldown;
    private float Damage;
    private List<Monster> targets = new List<Monster>();

    void Update()
    {
        CheckPositionChanges();
        Attack();
    }

    public void SetTowers(Tower2 A, Tower2 B, Tower2Manager Manager)
    {
        towerA = A;
        towerB = B;
        PrePosA = towerA.transform.position;
        PrePosB = towerB.transform.position;
        tower2Manager = Manager;
        AttackCooldown = towerA.attackCooldown;
        Damage = towerA.Damage;
    }

    private void Remove(Tower2Manager manager, ElectricPath elecPath)
    {
        List<ElectricPath> elecPaths = new List<ElectricPath>();
        foreach (ElectricPath path in manager.electricPaths)
        {
            if (path != elecPath)
            {
                elecPaths.Add(path);
            }
        }
        manager.electricPaths = elecPaths;
    }

    private void Remove(Monster Detarget)
    {
        List<Monster> Newtargets = new List<Monster>();
        foreach (Monster target in targets)
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
        if ((towerA.GetComponent<Tower2>().CanAlignedWith(towerB) || towerB.GetComponent<Tower2>().CanAlignedWith(towerA)) && (!Vector3.Equals(PrePosA, towerA.transform.position) || !Vector3.Equals(PrePosB, towerB.transform.position)))
        {
            //if (!Vector3.Equals(PrePosA, towerA.transform.position) || !Vector3.Equals(PrePosB, towerB.transform.position))
            //{
//                Debug.Log($"ChangePosition{towerA}{towerB}");
                PrePosA = towerA.transform.position;
                PrePosB = towerB.transform.position;
                transform.position = (PrePosA + PrePosB) / 2;
                Vector2 scale = transform.localScale;
                scale.x = Mathf.Abs(PrePosA.x - PrePosB.x) + Mathf.Abs(PrePosA.y - PrePosB.y);
                transform.localScale = scale;
           // }
        }
        else if(!towerA.GetComponent<Tower2>().CanAlignedWith(towerB))
        {
//            Debug.Log($"Destroy{towerA}{towerB}");
            Destroy(gameObject);
            Remove(tower2Manager, this);
            //towerA.Delect(towerA.electricPaths, gameObject);
            //towerB.Delect(towerB.electricPaths, gameObject);
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
        if (targets.Count > 0 && canAttack)
        {
            foreach(Monster target in targets)
            {
                target.attack(Damage);//对怪物造成伤害
                canAttack = false;
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Add(other.GetComponent<Monster>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Remove(other.GetComponent<Monster>());
        }
    }
}
