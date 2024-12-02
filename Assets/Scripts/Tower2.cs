using System.Linq;
using UnityEngine;

public class Tower2 : MonoBehaviour
{

    //private Vector3 position;
    [SerializeField]
    private GameObject Electricpath;

    public LayerMask layer;
    //攻击间隔、范围
    [SerializeField]
    private float AttackCooldown;
    public float attackCooldown
    {
        get { return AttackCooldown; }
    }
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
        //position = transform.position;
        if (FindObjectsOfType<Tower2Manager>().Count() < 1)
        {
            GameObject gameObject = new GameObject("Tower2manager");
            gameObject.AddComponent<Tower2Manager>();
            gameObject.GetComponent<Tower2Manager>().electricPathPrefab = Electricpath; 
        }
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

    /*public bool PositionChanged()
    {
        if(!Vector3.Equals(position, transform.position))
        {
            position = transform.position;
            return true;
        }
        return false;
    }*/

    public bool CanAlignedWith(Tower2 other)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = other.transform.position;
        bool IsalignedWith = (Mathf.Approximately(transform.position.x, other.transform.position.x) && Mathf.Abs(transform.position.y - other.transform.position.y) <= 8 && Mathf.Abs(transform.position.y - other.transform.position.y) > 1)
            || (Mathf.Approximately(transform.position.y, other.transform.position.y) && Mathf.Abs(transform.position.x - other.transform.position.x) <= 8 && Mathf.Abs(transform.position.x - other.transform.position.x) > 1);
        if (IsalignedWith)
        {
            // 使用Physics2D.Raycast检测塔之间的路径
            Vector3 dir = (endPos - startPos).normalized;
            RaycastHit2D hit = Physics2D.Raycast(startPos + dir * 0.6f, dir, Vector3.Distance(startPos, endPos), layer);

            // 如果射线检测到的物体不是目标塔，说明路径被其他物体阻挡
            if (hit.collider != null && hit.collider.gameObject != other.gameObject)
            {
                //Debug.Log("hasTower");
                return false;  // 路径有障碍物
            }
            return true;  // 路径畅通
        }
        return false;
    }

    /*public void SetElectricPathActive(bool isActive) 
    {
        if (electricPaths != null)
        {
            Debug.Log($"Electric path collider enabled: {isActive}");
            
            foreach(GameObject collider in electricPaths)
            {
                collider.enabled = isActive;
            }
            
        }
    }*/
   
}
