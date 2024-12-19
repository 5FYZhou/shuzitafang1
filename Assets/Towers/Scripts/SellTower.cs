using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellTower : MonoBehaviour
{
    private bool show = true;
    private float timer = 0;
    private Image image;
    private Button button;

    [SerializeField]
    private float sellPrice;

    private Tower parent;

    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        //image.enabled = false;
    }

    private void Update()
    {
        if (show)
        {
            image.enabled = true;
            button.enabled = true;

            timer += Time.deltaTime;
            if (timer >= 3f)
            {
                timer = 0;
                show = false;

                image.enabled = false;
                button.enabled = false;
            }
        }
    }

    public void Initialize(float sellprice,Tower Parent)
    {
        sellPrice = sellprice;
        parent = Parent;
    }

    public void Show()
    {
        show = true;
        timer = 0;
    }

    public void Sell()
    {
        Debug.Log("Sell"+sellPrice);
        parent.DestroyTower();
    }
}
