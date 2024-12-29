using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private LayerMask detectlayer;
    [SerializeField]
    private LayerMask towerLayer;

    private float price;

    public float Price { get => price; set => price = value; }

    private void Start()
    {
        //range = GetComponentInChildren<TowerAttackRange>();
        //if(this.gameObject.layer == 11)
        {
            //RemoveButton();
        }
    }

    public bool CanMoveToDir(Vector2 dir)
    {
        if(this.gameObject.layer == 13)
        {
            return false;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.6f, dir, 0.5f, detectlayer);
        if (!hit)
        {
            transform.Translate(dir);
            //ShowRange();
            return true;
        }
        
        return false;
    }

    public Vector2 TowerPosition()
    {
        return transform.position;
    }
    public void ToTowerPosition(Vector2 position0)
    {
        transform.position = position0;
    }


    public void TakeDamage(float damage)
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            float health = healthBar.ReduceHealthBar(damage);
            if (health <= 0)
            {
                if (this.gameObject.layer != 13)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("HomeBase is destroyed");
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    public void ShowRange()
    {
        TowerAttackRange range = GetComponentInChildren<TowerAttackRange>();
        if (range != null)
        {
            range.Show();
        }
    }
    public void ShowHealthBar()
    {
        HealthBar health = this.GetComponentInChildren<HealthBar>();
        if (health != null)
        {
            health.Show();
        }
    }
    public void ShowButton()
    {
        SellTower button = GetComponentInChildren<SellTower>();
        if (button != null)
        {
            button.Show();
        }
    }

    /*private void OnMouseDown()
    {
        //if (Input.GetMouseButtonDown(0))
        // 获取鼠标在屏幕上的位置
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 发射射线并检测是否击中带有碰撞体的Tower
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, towerLayer);

        if (hit.collider != null)
        {
            //Debug.Log("tower");
            Tower tower = hit.collider.GetComponent<Tower>();
            if (tower != null)
            {
                tower.ShowRange();
                tower.ShowHealthBar();
                tower.ShowButton();
            }
        }
    }*/

    public void DestroyTower()
    {
        Destroy(this.gameObject);
    }
}
