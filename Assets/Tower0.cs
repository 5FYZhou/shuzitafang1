using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower0 : MonoBehaviour
{
    private float Timer = 0;
    [SerializeField]
    private float Cooldown;
    [SerializeField]
    private float Amount;
    private bool Ready = false;
    void Update()
    {
        Produce();
    }

    private void Produce()
    {
        if (!Ready)
        {
            Timer += Time.deltaTime;
            if (Timer >= Cooldown)
            {
                Ready = true;
                Timer = 0;
            }
        }
        else
        {
            Debug.Log("produce");
            Ready = false;
        }
    }
}
