using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private GameObject aTowerPre;
    public GameObject ATowerPre { get => aTowerPre; }
    public float PerchasePrice { get => perchasePrice; }
    [SerializeField]
    private float perchasePrice;

    //private void Start()
    //{
        //perchasePrice = ATowerPre.GetComponent<Tower>().Price;
        //Debug.Log(perchasePrice);
    //}
}
