using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPscript : MonoBehaviour
{
    public float[] HP;
    IHealthAccessor accessor;
    // Start is called before the first frame update
    void Start()
    {
        HP = new float[2];
    }

    // Update is called once per frame
    void Update()
    {
        accessor = transform.parent.transform.parent.GetComponent<IHealthAccessor>();
        HP[0] = accessor.HP[0];
        HP[1] = accessor.HP[1];
        Vector2 scale = transform.localScale;
        scale.x = HP[0] / HP[1] * 1f;
        transform.localScale = scale;
    }
}
