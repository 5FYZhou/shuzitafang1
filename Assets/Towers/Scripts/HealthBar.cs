using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float InitialHealth;
    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
    }

    private float Timer = 0;
    [SerializeField]
    private float ShowCD;
    private Image render;
    private Image backgroundrender;
    public bool show = true;

    private void Start()
    {
        render = GetComponent<Image>();
        backgroundrender = transform.parent.GetComponent<Image>();
    }

    private void Update()
    {
        if (show)
        {
            render.enabled = true;
            backgroundrender.enabled = true;

            Timer += Time.deltaTime;
            if (Timer >= ShowCD)
            {
                Timer = 0;
                show = false;

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
        scale.x = health / InitialHealth * 1f;
        transform.localScale = scale;
        Show();
        return health;
    }

    public void ChangeHealth(float newhealth)
    {
        health = newhealth;
        Vector2 scale = transform.localScale;
        scale.x = health / InitialHealth * 1f;
        transform.localScale = scale;
    }

    public void Show()
    {
        show = true;
        Timer = 0;
    }
}
