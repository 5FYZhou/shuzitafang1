using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;
using System;

public class Equal : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private LayerMask Walllayer;
    [SerializeField]
    private LayerMask towerlayer;
    private Vector3 NewTowerPosition;
    private int[] SymbolsInACertainDirection = new int[4] { -1, -1, -1, -1 };//右下左上

    [SerializeField]
    private List<GameObject> TowerPrefabs = new List<GameObject>();

    private GameObject[] NewTower = new GameObject[4] { null, null, null, null };
    private bool[] HasInstantiate = new bool[4] { false, false, false, false };
    private float[] Timer = new float[4] { 0, 0, 0, 0 };

    void Start()
    {
        FindSymbols();
        player = FindObjectOfType<Player>();
       // Debug.Log(NewTowerPosition);
    }

    void Update()
    {
        FindTowerAB();
        CheckCoolDown();
    }

    private int JudgeSymbol(Collider2D gameObject)
    {
        if (gameObject.CompareTag("Plus"))
        {
            return 0;
        }
        else if (gameObject.CompareTag("Minus"))
        {
            return 1;
        }
        else if (gameObject.CompareTag("By"))
        {
            return 2;
        }
        else if (gameObject.CompareTag("Divide"))
        {
            return 3;
        }
        else if (gameObject.CompareTag("Surplus"))
        {
            return 4;
        }
        return -1;
    }

    private Vector3 Direction(int x)
    {
        if (x == 0) return Vector2.right;
        else if (x == 1) return Vector2.down;
        else if (x == 2) return Vector2.left;
        else if (x == 3) return Vector2.up;
        else return Vector2.zero;
    }

    private void FindSymbols()
    {
        for(int i = 0; i < 4; i++)
        {
            Vector3 dir = Direction(i);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.6f, dir, 1f, Walllayer);
            if (hit)
            {
                int symbol = JudgeSymbol(hit.collider);
                if (symbol >= 0)
                {
                    SymbolsInACertainDirection[i] = symbol;
                }
            }
        }
    }

    private int FindFirstDigit(string name)
    {
        Match match = Regex.Match(name, @"^\D*(\d)");
        if (match.Success)
        {
            // 将匹配到的字符转换为整数
            return int.Parse(match.Groups[1].Value);
        }
        return -1;
    }
    
    private int NewTowerDigit(int A,int B,int symbol)
    {
        if (symbol == 0) return (A + B) % 10;
        else if (symbol == 1) return (A - B) % 10;
        else if (symbol == 2) return (A * B) % 10;
        else if (symbol == 3) return (A / B) % 10;
        else if (symbol == 4) return (A % B) % 10;
        else return -1;
    }

    private bool CanProduceTower(Vector3 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir * 0.6f, dir, 0.5f, towerlayer);
        if (hit)
        {
            return false;
        }
        return true;
    }

    private GameObject HasTowerM(Vector3 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir * 0.6f, dir, 0.5f, towerlayer);
        if (hit)
        {
            if (hit.collider.CompareTag("TowerM"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private void SetTransparency(GameObject tower, float newAlpha)
    {
        Renderer renderer =tower.GetComponent<Renderer>();
        Color currentColor = renderer.material.color;
        renderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }

    private void FindTowerAB()
    {
        for (int i = 0; i < 4; i++)
        {
            int symboldigit = SymbolsInACertainDirection[i];
            Vector3 dir = Direction(i);
            NewTowerPosition = transform.position - dir;
            GameObject towerM = HasTowerM(-dir);
            if (symboldigit >= 0)//有符号
            {
                //Debug.Log(towerM);
                RaycastHit2D hitA = Physics2D.Raycast(transform.position + dir * 2 + dir * 0.6f, dir, 0.5f, towerlayer);
                RaycastHit2D hitB = Physics2D.Raycast(transform.position + dir * 2 + dir * -0.6f, dir * -1, 0.5f, towerlayer);
                if (hitA && hitB)//有AB
                {
                    //Debug.Log(i+"true");
                    int digitA = FindFirstDigit(hitA.collider.name);
                    int digitB = FindFirstDigit(hitB.collider.name);
                    if (digitA >= 0 && digitB >= 0)//是数字塔
                    {
                        //Debug.Log(digitA.ToString() + digitB.ToString()+symboldigit);
                        int newTowerDigit = NewTowerDigit(digitA, digitB, symboldigit);
                        if (!HasInstantiate[i] && CanProduceTower(-dir) && newTowerDigit >= 0 && newTowerDigit <= 3)//可以创造
                        {
                            //Debug.Log("creat");
                            NewTower[i] = Instantiate(TowerPrefabs[newTowerDigit], NewTowerPosition, Quaternion.identity);
                            HasInstantiate[i] = true;
                            NewTower[i].layer = 11;
                            SetTransparency(NewTower[i], 0.8f);//透明度
                        }
                        else if(!HasInstantiate[i] && towerM != null && !player.HasTower && newTowerDigit >= 0 && newTowerDigit <= 3)//towerM
                        {
                            //Debug.Log("MMMMM");
                            //Type towerType = Type.GetType("Tower"+newTowerDigit.ToString());
                            player.AddTower(newTowerDigit);
                            player.GiveI(this,i);
                            NewTower[i] = towerM;
                            HasInstantiate[i] = true;
                        }
                    }
                }
                else
                {
                    //Debug.Log(i+"false");
                    if (NewTower[i] && NewTower[i] != towerM)
                    {
                        //Debug.Log("Remove");
                        Destroy(NewTower[i].gameObject);
                        NewTower[i] = null;
                        HasInstantiate[i] = false;
                    }
                    else if (NewTower[i] != null && NewTower[i] == towerM)
                    {
                        //Debug.Log(i + "Des");
                        player.DelectTower();
                        NewTower[i] = null;
                        HasInstantiate[i] = false;
                    }
                }
            }
        }
    }

    private void CheckCoolDown()
    {
        for(int i = 0; i < 4; i++)
        {
            if (HasInstantiate[i] && NewTower[i] == null)
            {
                Timer[i] += Time.deltaTime;
                if (Timer[i] >= 10)
                {
                    HasInstantiate[i] = false;
                    Timer[i] = 0;
                }
            }
        }
    }

    public void FalseI(int i)
    {
        HasInstantiate[i] = false;
    }
}
