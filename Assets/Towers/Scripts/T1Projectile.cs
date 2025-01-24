using UnityEngine;
using System.Collections;

public class T1Projectile : MonoBehaviour
{
    //enemy enemy = new enemy();

    private GameObject target;

    private Tower1 parent;

    private float damage;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower1 parent, float damage)
    {
        this.target = parent.Target;
        this.parent = parent;
        this.damage = damage;
    }

    protected void MoveToTarget()
    {
        if(parent == null)
        {
            Destroy(gameObject);
        }
        if (target != null && !target.GetComponent<enemy>().Death)
        {
            Vector3 end = target.GetComponent<CapsuleCollider2D>().bounds.center;
            transform.position = Vector2.MoveTowards(transform.position, end, Time.deltaTime * parent.ProjectileSpeed);
           // Vector2 dir = target.transform.position - transform.position;
            Vector2 dir = transform.position - target.transform.position;
            float angle = - Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        /*else if (!target.isActive)
        {
            GameObject.Instance.Pool.ReleaseObject(gameObject);
        }怪物离开地图或死亡时子弹消失*/
        else
        { 
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            if (target.gameObject == other.gameObject)
            {
                animator.SetTrigger("Boom");
                //Debug.Log("Boom");
                //target.GetComponent<enemy>().attack(parent.Damage);
                // Destroy(gameObject);
            }
        }
    }

    public void AttackTarget()//动画中
    {
        target.GetComponent<enemy>().attack(damage);
    }

    public void DestroyGameObject()//动画中
    {
        Destroy(this.gameObject);
    }
}