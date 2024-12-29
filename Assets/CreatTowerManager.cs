using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatTowerManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask WallLayer;
    [SerializeField]
    private LayerMask towerLayer;

    private CurrencyManager currencyManager;

    private Vector2 mousePosition;
    private TowerButton clickedBtn;
    private bool MouseHaveATower;

    private void Start()
    {
        currencyManager = GameObject.Find("CurrencyCanvas").GetComponent<CurrencyManager>();
        //if (currencyManager != null)
        //{
            //Debug.Log(currencyManager);
        //}
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = StandardizePosition(mousePosition);
            //Debug.Log(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, WallLayer);

            if (hit.collider == null && clickedBtn != null && CanPerchase(clickedBtn))
            {

                //Debug.Log(clickedBtn.PerchasePrice);
                PlaceTower();
            }

            RaycastHit2D hitT = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, towerLayer);

            if (hitT.collider != null)
            {
                //Debug.Log("tower");
                Tower tower = hitT.collider.GetComponent<Tower>();
                if (tower != null)
                {
                    tower.ShowRange();
                    tower.ShowHealthBar();
                    tower.ShowButton();
                }
            }
        }
    }

    private Vector2 StandardizePosition(Vector2 position)
    {
        int x0 = Mathf.FloorToInt(position.x - (-9f));
        int y0 = Mathf.FloorToInt(position.y - (-6.5f));
        position.x =-9f + x0 + 0.5f;
        position.y = -6.5f + y0 + 0.5f;
        return position;
    }

    public void PickTower(TowerButton towerBtn)
    {
        //Debug.Log("clicked");
        this.clickedBtn = towerBtn;
    }

    private void PlaceTower()
    {
        
        {
            currencyManager.Currency -= clickedBtn.PerchasePrice;
            Instantiate(clickedBtn.ATowerPre, mousePosition, Quaternion.identity);
            clickedBtn = null;
        }
    }

    private bool CanPerchase(TowerButton btn)
    {
        float price = btn.PerchasePrice;
        if (currencyManager.Currency >= price)
        {
            return true;
        }
        return false;
    }

}
