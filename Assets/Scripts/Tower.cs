using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private LayerMask detectlayer;
    TowerAttackRange range;
    private float ShowRangeTimer;
    public bool Ismoved;

    private void Start()
    {
        //range = GetComponentInChildren<TowerAttackRange>();
        ShowRangeTimer = 0;
    }

    private void Update()
    {
        if (Ismoved)
        {
            ShowRangeTimer += Time.deltaTime;
            if (ShowRangeTimer >= 3f)
            {
                DisActiveRange();
                ShowRangeTimer = 0;
                Ismoved = false;
            }
        }
    }

    public bool CanMoveToDir(Vector2 dir)
    {
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


    public void TakeDamege(float damage)
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            float health = healthBar.ReduceHealthBar(damage);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ShowRange()
    {
        range = GetComponentInChildren<TowerAttackRange>();
        if (range != null)
        {
            range.Active();
            Ismoved = true;
            ShowRangeTimer = 0;
            //Invoke("DisActiveRange", 3f);
        }
    }

    private void DisActiveRange()
    {
        if (range != null)
        {
            range.Disactive();
        }
    }
}
