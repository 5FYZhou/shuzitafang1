using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreatTower : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> TowerPrefabs = new List<GameObject>();

    private bool MouseHaveATower;

    private void OnMouseDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = StandardizePosition(mousePosition);
        Debug.Log(mousePosition);
        /*if (MouseHaveTower)
        {
            Debug.Log("Mouse");
            // 获取鼠标在屏幕上的位置
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, WallLayer);
            if (hit.collider == null)
            {
                Instantiate(ATowerPre, mousePosition, Quaternion.identity);
            }
        }*/
    }

    private Vector2 StandardizePosition(Vector2 position)
    {
        position.x = Mathf.FloorToInt(position.x);
        position.y = Mathf.FloorToInt(position.y);
        return position;
    }
}
