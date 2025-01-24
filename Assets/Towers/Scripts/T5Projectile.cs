using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T5Projectile : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private LayerMask enemyLayer;

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
        if (target != null && !target.GetComponent<enemy>().Death/*怪物活着*/)
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
                //Debug.Log("Boom");
                Attack();
                target.GetComponent<enemy>().attack(parent.Damage);
                Destroy(gameObject);
            }
        }
    }

    private Vector2 StandardizePosition(Vector2 position, float _x, float _y)
    {
        int x0 = Mathf.FloorToInt(position.x - _x);
        int y0 = Mathf.FloorToInt(position.y - _y);
        position.x = _x + x0 + 0.5f;
        position.y = _y + y0 + 0.5f;
        return position;
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

    private void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(StandardizePosition(gameObject.transform.position, -9f, -6.5f), new Vector2(3f, 3f), 0f, enemyLayer);
        //Debug.Log(colliders.Length);
        foreach (var ttarget in colliders)
        {
            if (ttarget != target)
            {
                ttarget.GetComponent<enemy>().attack(parent.SplashDamage);
            }
            //Debug.Log(target.name + parent.Damage.ToString());
        }
    }

    // 绘制3x3范围的矩形框
    private void OnDrawGizmos()
    {
        // 获取方块1的中心位置
        
            Vector2 center =StandardizePosition(gameObject.transform.position, -9f, -6.5f);
        //Debug.Log(center);
            // 设置 Gizmos 颜色（这里用红色）
            Gizmos.color = Color.green;

        // 绘制矩形框（宽3，高3）
        Gizmos.DrawWireCube(center, new Vector2(3f, 3f));
        
    }
}