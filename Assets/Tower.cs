using UnityEngine;

public class Tower : MonoBehaviour
{
    public LayerMask detectlayer;

    //ÑªÁ¿
    [SerializeField]
    private float initialHealthVolume;
    public float InitialHealthVolume
    {
        get { return initialHealthVolume; }
    }
    //¼Û¸ñ
    [SerializeField]
    private float PurchasePrice;
    [SerializeField]
    private float SellingPrice;

    void Start()
    {
        GiveHealthVolumn();
    }

    public bool CanMoveToDir(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.6f, dir, 0.5f, detectlayer);
        if (!hit)
        {
            transform.Translate(dir);
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

    private void GiveHealthVolumn()
    {
        HealthBar healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(InitialHealthVolume);
        }
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
}
