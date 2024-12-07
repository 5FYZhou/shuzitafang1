using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 moveDir;
    public LayerMask detectlayer;
    private Vector2 position0;

    public bool HasTower;
    [SerializeField]
    private List<GameObject> TowerPrefabs = new List<GameObject>();
    private GameObject NewTower = null;
    private float Timer = 0;

    private Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            moveDir = Vector2.right;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            moveDir = Vector2.left;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            moveDir = Vector2.up;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            moveDir = Vector2.down;

        if (moveDir != Vector2.zero)
        {
            if (CanMoveToDir(moveDir))
            {
                Move(moveDir);
            }
        }
        moveDir = Vector2.zero;

        AddTowerCoolDown();
    }

    bool CanMoveToDir(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, detectlayer);
        if (!hit)
            return true;
        else
        {
            Tower box = hit.collider.GetComponent<Tower>();
            if (box != null)
            {
                box.ShowRange();
                //Debug.Log($"{hit.collider.name}show");
                if (box.CanMoveToDir(dir))
                    return true;
                else
                {
                    position0 = transform.position;
                    transform.position = box.TowerPosition();
                    box.ToTowerPosition(position0);
                }
            }
        }
        return false;
    }

    void Move(Vector2 dir)
    {
        transform.Translate(dir);
        ShowTowerRange();
    }

    public void AddTower(int towerdigit)
    {
        if (!HasTower)
        {
            Debug.Log("Ins");
            NewTower = Instantiate(TowerPrefabs[towerdigit], this.transform.position, Quaternion.identity);
            NewTower.transform.SetParent(this.transform);
            /*
            Vector2 scale = NewTower.transform.localScale;
            scale.x = 0.5f;scale.y = 0.5f;
            NewTower.transform.localScale = scale;*/
            HasTower = true;
            NewTower.GetComponent<Renderer>().enabled = false;
            NewTower.layer = 12;
        }
    }

    public void DelectTower()
    {
        if (HasTower && NewTower != null)
        {
            Debug.Log("Destroy");
            Destroy(NewTower);
            HasTower = false;
        }
    }

    private void AddTowerCoolDown()
    {
        if(HasTower && NewTower == null)
        {
            Timer += Time.deltaTime;

            render.material.color = new Color(150, 150, 150);

            if (Timer >= 10)
            {
                HasTower = false;
                Timer = 0;
                render.material.color = new Color(0, 0, 0);
            }
        }
    }

    private void ShowTowerRange()
    {
        if (HasTower && NewTower != null)
        {
            Tower tower = NewTower.GetComponent<Tower>();
            if (tower != null)
            {
                tower.ShowRange();
            }
        }
    }
}
