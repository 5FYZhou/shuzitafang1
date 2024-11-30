using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    public void SetAttackRange(Vector2 range)
    {
        this.transform.localScale = range;
    }
}
