using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatTowerManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask CannotPlaceLayer;
    [SerializeField]
    private LayerMask towerLayer;

    private CurrencyManager currencyManager;
    private Hover hover;

    private Vector2 mousePosition;
    private TowerButton clickedBtn;
    //场上塔的数量上限
    [SerializeField]
    private int maxTowerNumber;
    public int MaxTowerNumber { get => maxTowerNumber; set => maxTowerNumber = value; }
    private int towerNumber = 0;
    public int TowerNumber { get => towerNumber; set => towerNumber = value; }
    //建造塔的数量上限
    [SerializeField]
    private int MaxCreatedTowerNumber;
    private int CreatedTowerNumber = 0;
    private bool towersNumHasReachedMax;
    public bool TowersNumHasReachedMax { get => towersNumHasReachedMax; set => towersNumHasReachedMax = value; }

    private void Start()
    {
        currencyManager = GameObject.Find("CurrencyCanvas").GetComponent<CurrencyManager>();
        hover = GameObject.Find("Hover").GetComponent<Hover>();
        //if (currencyManager != null)
        //{
        //Debug.Log(currencyManager);
        //}
    }

    private void Update()
    {
        HandleEscape();
        CheckTowerNum();
    }

    private void OnMouseDown()
    {
        Debug.Log("down");
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("event");
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = StandardizePosition(mousePosition, -9f, -6.5f);
            Debug.Log(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, CannotPlaceLayer);

            if (hit.collider == null && clickedBtn != null && CanPerchase(clickedBtn))
            {
                PlaceTower();
            }

            //点击显示塔状态
            RaycastHit2D hitT = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, towerLayer);
            Debug.Log(hitT);
            if (hitT.collider != null)
            {
                Debug.Log("hit");
                Tower tower = hitT.collider.GetComponent<Tower>();
                if (tower != null)
                {
                    Debug.Log("tower");
                    tower.ShowRange();
                    tower.ShowHealthBar();
                    tower.ShowButton();
                }
            }
        }
    }

    private Vector2 StandardizePosition(Vector2 position, float _x, float _y)
    {
        int x0 = Mathf.FloorToInt(position.x - _x);
        int y0 = Mathf.FloorToInt(position.y - _y);
        position.x = _x + x0 + 0.5f;
        position.y = _y + y0 + 0.5f;
        return position;
    }

    public void PickTower(TowerButton towerBtn)
    {
        //Debug.Log("clicked");
        this.clickedBtn = towerBtn;

        hover.Activate(clickedBtn.Sprite);
    }

    private void PlaceTower()
    {
        currencyManager.Currency -= clickedBtn.PerchasePrice;
        Instantiate(clickedBtn.ATowerPre, mousePosition, Quaternion.identity);
        clickedBtn = null;
        hover.Deactivate();

        TowerNumber += 1;
        CreatedTowerNumber += 1;
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

    private void HandleEscape()
    {
        if (Input.GetMouseButtonDown(1))
        {
            hover.Deactivate();
            clickedBtn = null;
        }
    }

    private void CheckTowerNum()
    {
        //Debug.Log(TowerNumber);
        if (CreatedTowerNumber >= MaxCreatedTowerNumber && !TowersNumHasReachedMax)
        {
            TowersNumHasReachedMax = true;
            Debug.Log("建造塔的数量达到10！");
        }
        /*
        if (TowerNumber >= MaxTowerNumber && !BtnGrey)
        {
            Debug.Log("塔的数量达到上限15！");
            TowerButton[] towerBts = GameObject.FindObjectsOfType<TowerButton>();
            foreach( TowerButton btn in towerBts)
            {
                btn.Grey();
            }
            BtnGrey = true;
        }
        else if(TowerNumber < MaxTowerNumber && BtnGrey)
        {
            TowerButton[] towerBts = GameObject.FindObjectsOfType<TowerButton>();
            foreach (TowerButton btn in towerBts)
            {
                btn.White();
            }
            BtnGrey = false;
        }*/
    }
}
