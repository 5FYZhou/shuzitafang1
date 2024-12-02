using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Monster target;

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
            /*//子弹方向
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            */

        }
        /*else if (!target.isActive)
        {
            GameObject.Instance.Pool.ReleaseObject(gameObject);
        }怪物离开地图或死亡时子弹消失*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (target.gameObject == other.gameObject)
            {
                target.attack(parent.Damage);//对怪物造成伤害
                Destroy(gameObject);
            }
        }
    }
}
