using UnityEngine;

public class Tower : MonoBehaviour
{
    protected LayerMask CannotMoveLayer;

    //血量
    protected float InitialHealthVolume;
    //价格
    protected float PurchasePrice;
    protected float SellingPrice;
    //攻击
    protected float AttackPower;
    protected float Range;

    protected Animator animator;

    public bool TCanMoveToDir(Vector2 dir)
    {
        CannotMoveLayer = LayerMask.GetMask("Wall", "Tower", "EqualTower", "PlayerTower", "Homebase");

        if (this.gameObject.layer == 13)
        {
            return false;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.6f, dir, 0.5f, CannotMoveLayer);
        //Debug.Log(hit.collider);
        if (!hit)
        {
            transform.Translate(dir);
            return true;
        }
        
        return false;
    }

    public Vector2 TowerPosition()
    {
        return this.transform.position;
    }
    public void ToTowerPosition(Vector2 position0)
    {
        this.transform.position = position0;
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

    protected void CreatRange(GameObject prefab)
    {
        GameObject towerRange = Instantiate(prefab, this.transform.position, Quaternion.identity);
        towerRange.transform.SetParent(this.transform);

        TowerAttackRange towerrange=GetComponentInChildren<TowerAttackRange>();
        if (towerrange)
        {
            towerrange.SetAttackRange(new Vector2(Range, Range));
        }
    }
    protected void CreatHealthBar(GameObject prefab)
    {
        Vector3 position = new(transform.position.x, transform.position.y + 1f, 0f);
        GameObject bar = Instantiate(prefab, position, Quaternion.identity);
        bar.transform.SetParent(this.transform);
        Vector2 scale = bar.transform.localScale;
        scale *= 0.8f;
        bar.transform.localScale = scale;

        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(InitialHealthVolume);
        }
    }
    protected void CreatButton(GameObject prefab)
    {
        if (gameObject.layer != 11 && gameObject.layer != 12)
        {
            Vector3 position = new(transform.position.x, transform.position.y - 0.7f, 0f);
            GameObject button = Instantiate(prefab, position, Quaternion.identity);
            button.transform.SetParent(this.transform);
            Vector2 scale = button.transform.localScale;
            scale *= 0.8f;
            button.transform.localScale = scale;

            SellTower sellTower = GetComponentInChildren<SellTower>();
            if (sellTower != null)
            {
                sellTower.Initialize(SellingPrice);
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

    public void StrengthenAttackPower(float per)
    {
        //Debug.Log("S");
        AttackPower *= (per + 1);
    }
    public void ReduceAttackPower(float per)
    {
        AttackPower /= (per + 1);
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

    /*public void DestroyTower()
    {
        Destroy(this.gameObject);
    }*/
}
