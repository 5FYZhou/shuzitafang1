using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    public void SetAttackRange(Vector2 range)
    {
        this.transform.localScale = range;
    }

    public void Active()
    {
        spriteRenderer.enabled = true;
    }

    public void Disactive()
    {
        spriteRenderer.enabled = false;
    }
}
