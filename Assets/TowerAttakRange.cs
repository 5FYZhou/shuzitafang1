using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    public void SetAttackRange(Vector2 range)
    {
        this.transform.localScale = range;
    }
}
