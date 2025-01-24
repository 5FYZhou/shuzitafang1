using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private bool show = true;
    private float timer = 0;
    [SerializeField]
    private float ShowCD;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (show)
        {
            Active();

            timer += Time.deltaTime;
            if (timer >= ShowCD)
            {
                timer = 0;
                show = false;

                DisActive();
            }
        }
    }

    public void SetAttackRange(Vector2 range)
    {
        this.transform.localScale = range;
    }

    public void Active()
    {
        spriteRenderer.enabled = true;
    }

    public void DisActive()
    {
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        timer = 0;
        show = true;
    }
}
