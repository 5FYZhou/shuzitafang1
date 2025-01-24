using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeBase : Tower
{
    [SerializeField]
    private GameObject healthBarPrefab;

    //Ѫ��
    [SerializeField]
    private float initialHealthVolume;
    

    private void Awake()
    {
        InitialHealthVolume = initialHealthVolume;
        CreatHealthBar(healthBarPrefab, 0.8f, 1f);
    }
}
