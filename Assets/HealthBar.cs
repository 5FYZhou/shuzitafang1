using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float InitialHealth;
    private float health;

    public void SetHealth(float Health)
    {
        InitialHealth = Health;
        health = InitialHealth;
    }

    public float ReduceHealthBar(float damage)
    {
        health -= damage;
        Vector2 scale = transform.localScale;
        scale.x = health/InitialHealth*1f;
        transform.localScale = scale;
        return health;
    }

}
