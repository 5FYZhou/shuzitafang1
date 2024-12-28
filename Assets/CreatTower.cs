using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatTower : MonoBehaviour
{
    [SerializeField]
    private GameObject ATowerPre;
    [SerializeField]
    private LayerMask WallLayer;

    private bool MouseHaveTower;

    public void HaveATower()
    {
        Debug.Log("have");
        MouseHaveTower = true;
    }

    
}
