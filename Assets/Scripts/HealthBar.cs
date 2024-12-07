using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float InitialHealth;
    private float health;

    private float Timer = 0;
    private Image render;
    private Image backgroundrender;
    private bool IsAttacked = true;

    private void Start()
    {
        render = GetComponent<Image>();
        backgroundrender = GetComponentInParent<Image>();
    }

    private void Update()
    {
        if (IsAttacked)
        {
            render.enabled = true;
            backgroundrender.enabled = true;

            Timer += Time.deltaTime;
            if (Timer >= 3)
            {
                Timer = 0;
                IsAttacked = false;

                render.enabled = false;
                backgroundrender.enabled = false;
            }
        }
    }

    public void SetHealth(float Health)
    {
        InitialHealth = Health;
        health = InitialHealth;
    }

    public float ReduceHealthBar(float damage)
    {
        health = Mathf.Clamp(health - damage, 0f,InitialHealth);
        Vector2 scale = transform.localScale;
        scale.x = health/InitialHealth*1f;
        transform.localScale = scale;
        IsAttacked = true;
        Timer = 0;
        return health;
    }
}
