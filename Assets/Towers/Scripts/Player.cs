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
    public GameObject NewTower = null;
    //private float Timer = 0;
    //private int I = -1;
    //private Equal EqualI;

    private Renderer render;

    private Animator animator;

    private void Start()
    {
        render = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            moveDir = Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            moveDir = Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            moveDir = Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            moveDir = Vector2.down;
        }

        SetDirAnimation(moveDir);

        if (moveDir != Vector2.zero)
        {
            if (CanMoveToDir(moveDir))
            {
                Move(moveDir);
            }
        }
        moveDir = Vector2.zero;

        //AddTowerCoolDown();
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
                box.ShowHealthBar();
                //Debug.Log($"{hit.collider.name}show");
                if (box.CanMoveToDir(dir))
                    return true;
                else
                {
                    position0 = transform.position;
                    transform.position = box.TowerPosition();
                    box.ToTowerPosition(position0);

                    SetDirAnimation((Vector3)position0 - transform.position);
                }
            }
        }
        return false;
    }

    void Move(Vector2 dir)
    {
        transform.Translate(dir);
        ShowTower();
    }

    public void AddTower(int towerdigit)
    {
        if (!HasTower)
        {
            //Debug.Log("Ins");
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

    public void AddPreTower(GameObject preTower)
    {
        if (!HasTower)
        {
            NewTower = preTower;
            NewTower.transform.SetParent(this.transform);
            HasTower = true;
            NewTower.GetComponent<Renderer>().enabled = false;
            NewTower.layer = 12;
        }
    }

    public void DelectTower()
    {
        if (HasTower && NewTower != null)
        {
            //Debug.Log("Destroy");
            Destroy(NewTower);
            HasTower = false;
        }
    }
    /*
    private void AddTowerCoolDown()
    {
        if(HasTower && NewTower == null)
        {
            EqualI.FalseI(I);
            Timer += Time.deltaTime;

            //SetTransparency(180);
            //this.GetComponent<Renderer>().material.color = new Color(180, 180, 180, 255);

            if (Timer >= 10)
            {
                HasTower = false;
                Timer = 0;
                //SetTransparency(255);
            }
        }
    }*/

    private void ShowTower()
    {
        if (HasTower && NewTower != null)
        {
            Tower tower = NewTower.GetComponent<Tower>();
            if (tower != null)
            {
                tower.ShowRange();
                tower.ShowHealthBar();
            }
        }
    }

    private void SetTransparency(float newAlpha)
    {
        Renderer renderer = this.GetComponent<Renderer>();
        Color currentColor = renderer.material.color;
        renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }
    /*
    public void GiveI(Equal equal, int i)
    {
        EqualI = equal;
        I = i;
    }*/

    private void SetDirAnimation(Vector3 dir)
    {
        dir.Normalize();
        if (dir == Vector3.right)
        {
            animator.SetBool("Right", true);
            animator.SetBool("Down", false);
            animator.SetBool("Up", false);
            animator.SetBool("Left", false);
        }
        if (dir == Vector3.left)
        {
            animator.SetBool("Left", true);
            animator.SetBool("Down", false);
            animator.SetBool("Right", false);
            animator.SetBool("Up", false);
        }
        if (dir == Vector3.up)
        {
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
        }
        if (dir == Vector3.down)
        {
            animator.SetBool("Down", true);
            animator.SetBool("Right", false);
            animator.SetBool("Up", false);
            animator.SetBool("Left", false);
        }
    }
}
