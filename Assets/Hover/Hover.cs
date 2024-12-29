using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject colorSquarePre;
    private GameObject colorSquare;
    private SpriteRenderer colorSquareRenderer;

    private Color greencolor;
    [SerializeField]
    private LayerMask CannotPlaceLayer;
    private Vector2 placePos;
    private GameObject colorsquare;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            ShowTileColor();
        }
    }

    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
        if (colorSquare != null)
        {
            colorSquare.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void Deactivate()
    {
        this.spriteRenderer.sprite = null;
        spriteRenderer.enabled = false;

        if (colorSquare != null)
        {
            colorSquare.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(colorSquare.gameObject);
        }
    }

    private void ShowTileColor()
    {
        if (StandardizePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)) != placePos)
        {
            placePos = StandardizePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (colorSquare == null)
            {
                colorSquare = Instantiate(colorSquarePre, placePos, Quaternion.identity);
            }
            else
            {
                colorSquare.transform.position = placePos;
            }
        }
        
        if (CanPlace(placePos))
        {
            colorSquare.GetComponent<SpriteRenderer>().color = new Color32(90, 255, 90, 160);
        }
        else
        {
            colorSquare.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 160);
        }
    }

    private Vector2 StandardizePosition(Vector2 position)
    {
        int x0 = Mathf.FloorToInt(position.x - (-9f));
        int y0 = Mathf.FloorToInt(position.y - (-6.5f));
        position.x = -9f + x0 + 0.5f;
        position.y = -6.5f + y0 + 0.5f;
        return position;
    }

    private bool CanPlace(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, CannotPlaceLayer);
        if(hit.collider == null)
        {
            return true;
        }
        return false;
    }
}
