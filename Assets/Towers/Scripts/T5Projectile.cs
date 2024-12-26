using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T5Projectile : MonoBehaviour
{
    private GameObject target;

    private Tower5 parent;

    private Animator animator;

    private void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower5 parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }

    protected void MoveToTarget()
    {
        if (parent == null)
        {
            Destroy(gameObject);
        }
        if (target != null/*&&target.IsActive怪物活着*/)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
            // Vector2 dir = target.transform.position - transform.position;
            Vector2 dir = transform.position - target.transform.position;
            float angle = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
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
                //animator.SetTrigger("Boom");
                Debug.Log("Boom");
                target.GetComponent<enemy>().attack(parent.Damage);
                Destroy(gameObject);
            }
        }
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
