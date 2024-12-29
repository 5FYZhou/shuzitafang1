using System.Linq;
using UnityEngine;

public class Tower2 : MonoBehaviour, IStrengthenTowerAttackPower
{
    [SerializeField]
    private GameObject attackRangePrefab;
    [SerializeField]
    private GameObject healthBarPrefab;
    private GameObject towerRange;
    private GameObject bar;
    [SerializeField]
    private GameObject sellButtonPrefab;
    private GameObject button;

    [SerializeField]
    private GameObject Electricpath;
    [SerializeField]
    private Sprite electricSprite;

    [SerializeField]
    private LayerMask layer;
    //�����������Χ
    [SerializeField]
    private float AttackCooldown;
    public float attackCooldown
    {
        get { return AttackCooldown; }
    }
    [SerializeField]
    private float AttackRange;
    //������
    [SerializeField]
    private float AttackPower;
    public float Damage
    {
        get { return AttackPower; }
    }

    //Ѫ��
    [SerializeField]
    private float initialHealthVolume;
    public float InitialHealthVolume
    {
        get { return initialHealthVolume; }
    }
    //�۸�
    [SerializeField]
    private float purchasePrice;
    public float PurchasePrice
    {
        get { return purchasePrice; }
    }
    [SerializeField]
    private float sellingPrice;
    public float SellingPrice
    {
        get { return sellingPrice; }
    }

    public Animator animator;

    private void Awake()
    {
        CreatChild();
        GetComponent<Tower>().Price = purchasePrice;
    }

    void Start()
    {
        //CreatChild();
        CreatButton();
        //position = transform.position;
        if (FindObjectsOfType<Tower2Manager>().Count() < 1)
        {
            GameObject gameObject = new GameObject("Tower2manager");
            gameObject.AddComponent<Tower2Manager>();
            gameObject.GetComponent<Tower2Manager>().electricPathPrefab = Electricpath;
            gameObject.GetComponent<Tower2Manager>().electricSprite = electricSprite;
        }

        animator = GetComponent<Animator>();

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

    private void GiveHealthVolumn()
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(InitialHealthVolume);
        }
    }

    private void GiveSellButton()
    {
        SellTower sellTower = GetComponentInChildren<SellTower>();
        if (sellTower != null)
        {
            sellTower.Initialize(sellingPrice, GetComponent<Tower>());
        }
    }

    private void CreatChild()
    {
        Transform HealthBar = transform.Find("HealthBarCanvas");
        if (!HealthBar)
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1f, 0f);
            bar = Instantiate(healthBarPrefab, position, Quaternion.identity);
            bar.transform.SetParent(this.transform);
            Vector2 scale = bar.transform.localScale;
            scale = scale * 0.8f;
            bar.transform.localScale = scale;
            GiveHealthVolumn();
        }
        Transform AttackRange = transform.Find("TowerAttakRange");
        if (!AttackRange)
        {
            towerRange = Instantiate(attackRangePrefab, this.transform.position, Quaternion.identity);
            towerRange.transform.SetParent(this.transform);
            GiveAttackRange();
        }
    }

    private void CreatButton()
    {
        if (gameObject.layer != 11 && gameObject.layer != 12)
        {
            Transform SellButton = transform.Find("SellButtonCanvas");
            if (!SellButton)
            {
                Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.7f, 0f);
                button = Instantiate(sellButtonPrefab, position, Quaternion.identity);
                button.transform.SetParent(this.transform);
                Vector2 scale = button.transform.localScale;
                scale = scale * 0.8f;
                button.transform.localScale = scale;
                GiveSellButton();
            }
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
        if (this && other)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = other.transform.position;
            bool IsalignedWith = (Mathf.Approximately(transform.position.x, other.transform.position.x) && Mathf.Abs(transform.position.y - other.transform.position.y) <= 8 && Mathf.Abs(transform.position.y - other.transform.position.y) > 1)
                || (Mathf.Approximately(transform.position.y, other.transform.position.y) && Mathf.Abs(transform.position.x - other.transform.position.x) <= 8 && Mathf.Abs(transform.position.x - other.transform.position.x) > 1);
            if (IsalignedWith)
            {
                // ʹ��Physics2D.Raycast�����֮���·��
                Vector3 dir = (endPos - startPos).normalized;
                RaycastHit2D hit = Physics2D.Raycast(startPos + dir * 0.6f, dir, Vector3.Distance(startPos, endPos), layer);

                // ������߼�⵽�����岻��Ŀ������˵��·�������������赲
                if (hit.collider != null && hit.collider.gameObject != other.gameObject)
                {
                    //Debug.Log("hasTower");
                    return false;  // ·�����ϰ���
                }
                return true;  // ·����ͨ
            }
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

    public void Strengthen(float per)
    {
        AttackPower = AttackPower * (per + 1);
    }
    public void Reduce(float per)
    {
        AttackPower = AttackPower / (per + 1);
    }
}
