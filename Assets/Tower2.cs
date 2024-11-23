using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2 : MonoBehaviour
{
    // 触发电流路径的Collider
    public BoxCollider2D electricPathCollider;  // 用于电流路径的Collider
    public LayerMask layer;
    //攻击间隔、范围
    private float AttackTimer = 0;
    [SerializeField]
    private float AttackCooldown;
    [SerializeField]
    private float AttackRange;
    //攻击力
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

    public bool IsAlignedWith(Tower2 other)
    {
        // 判断塔是否在同一水平线或垂直线上
        return (Mathf.Approximately(transform.position.x, other.transform.position.x) && Mathf.Abs(transform.position.y - other.transform.position.y) <= 8)
            || (Mathf.Approximately(transform.position.y, other.transform.position.y) && Mathf.Abs(transform.position.x - other.transform.position.x) <= 8);
    }

    public void SetElectricPathActive(bool isActive)
    {
        if (electricPathCollider != null)
        {
            Debug.Log($"Electric path collider enabled: {isActive}");
            electricPathCollider.enabled = isActive;
        }
    }

    public bool IsPathClear(Tower2 other)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = other.transform.position;

        // 使用Physics2D.Raycast检测塔之间的路径
        RaycastHit2D hit = Physics2D.Raycast(startPos + (endPos - startPos) * 0.5f, endPos - startPos, Vector3.Distance(startPos, endPos),layer);

        // 如果射线检测到的物体不是目标塔，说明路径被其他物体阻挡
        if (hit.collider != null && hit.collider.gameObject != other.gameObject)
        {
            return false;  // 路径有障碍物
        }

        return true;  // 路径畅通
    }
    // 更新电流路径的位置、大小和旋转
    public void UpdateElectricPath(Tower other)
    {
        if (electricPathObject == null) return;

        Vector3 startPos = transform.position;
        Vector3 endPos = other.transform.position;

        // 更新电流路径的位置
        electricPathObject.transform.position = (startPos + endPos) / 2;

        // 计算电流路径的长度
        float distance = Vector3.Distance(startPos, endPos);
        electricPathCollider.size = new Vector2(distance, electricPathCollider.size.y);  // 修改Collider的大小以匹配新长度

        // 计算电流路径的角度
        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * Mathf.Rad2Deg;
        electricPathObject.transform.rotation = Quaternion.Euler(0, 0, angle);  // 设置路径的旋转
    }
}
