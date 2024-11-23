using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 moveDir;
    public LayerMask detectlayer;
    private Vector2 position0;

    // Update is called once per frame
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
    }

    bool CanMoveToDir(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir,0.5f,detectlayer);
        if (!hit)
            return true;
        else
        {
            Tower box = hit.collider.GetComponent<Tower>();
            if (box != null)
                if (box.CanMoveToDir(dir))
                    return true;
                else
                {
                    position0 = transform.position;
                    transform.position = box.TowerPosition();
                    box.ToTowerPosition(position0);
                }
        }
        return false;
    }
    void Move(Vector2 dir)
    {
        transform.Translate(dir);
    }
}
