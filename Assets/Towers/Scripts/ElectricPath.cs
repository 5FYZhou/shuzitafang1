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
    private List<GameObject> targets = new List<GameObject>();

    private Animator towerAanimator;
    private Animator towerBanimator;

    void Update()
    {
        CheckPositionChanges();
        Attack();
        //Debug.Log(targets.Count);
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
        towerAanimator = towerA.animator;
        towerBanimator = towerB.animator;
    }

    private void Remove(GameObject Detarget)
    {
        List<GameObject> Newtargets = new List<GameObject>();
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
                Vector2 scale = transform.localScale;
                scale.x = Mathf.Abs(PrePosA.x - PrePosB.x) + Mathf.Abs(PrePosA.y - PrePosB.y) - 1f;
                transform.localScale = scale;
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

    public void AddSprites(Sprite electricSprite)
    {

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
}
