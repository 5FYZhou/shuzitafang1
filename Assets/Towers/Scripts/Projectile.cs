using UnityEngine;

public class Projectile : MonoBehaviour
{
    //enemy enemy = new enemy();

    private GameObject target;

    private Tower1 parent;

    void Update()
    {
        MoveToTarget();
    }

    public void Initialize(Tower1 parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }

    private void MoveToTarget()
    {
        if (target != null/*&&target.IsActive怪物活着*/)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
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
                target.GetComponent<enemy>().attack(parent.Damage);
                Destroy(gameObject);
            }
        }
    }
}
