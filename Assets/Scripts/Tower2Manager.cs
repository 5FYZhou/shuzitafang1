using System.Collections.Generic;
using UnityEngine;

public class Tower2Manager : MonoBehaviour
{
    private List<Tower2> towers = new List<Tower2>();  // 存储所有的塔
    public List<ElectricPath> electricPaths = new List<ElectricPath>();
    public GameObject electricPathPrefab;  // 电流路径的预制件

    //private void Start()
    //{
        // 初始化时，查找所有塔
        //towers.AddRange(FindObjectsOfType<Tower2>());
        //Debug.Log(towers.Count);
    //}

    private void Update()
    {
        //if (Tower2CountChanged() || Tower2PositionChanged())
        //{
        CheckTower2CountChange();
        CheckElectricCurrentBetweenTowers();
        //}
    }

    private void CheckTower2CountChange()
    {
        Tower2[] newtowers = FindObjectsOfType<Tower2>();
        if (newtowers.Length != towers.Count)
        {
            towers.Clear();
            towers.AddRange(newtowers);
        }
    }

    /*private bool Tower2PositionChanged()
    {
        foreach(Tower2 tower2 in towers)
        {
            if (tower2.PositionChanged())
            {
                return true;
            }
        }
        return false;
    }*/

    // 检查是否有两个塔产生电流
    private void CheckElectricCurrentBetweenTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = i + 1; j < towers.Count; j++)
            {
                Tower2 towerA = towers[i];
                Tower2 towerB = towers[j];
                if (towerA.CanAlignedWith(towerB) && !IsElectricityAlreadyExists(towerA,towerB))  // 如果塔在同一水平或垂直线上，并且路径畅通 +没有电流
                {
                    // 创建电流路径
                    //Debug.Log($"creat between {towerA} and {towerB}");
                    CreateElectricPathCollider(towerA, towerB);
                }
                //else
                //{
                    // 禁用电流路径
                    //towerA.SetElectricPathActive(false);
                    //towerB.SetElectricPathActive(false);
                //}
            }
        }
    }

    // 创建电流路径的Collider
    private void CreateElectricPathCollider(Tower2 towerA, Tower2 towerB)
    {

            // 先禁用所有路径
            //towerA.SetElectricPathActive(false);
            //towerB.SetElectricPathActive(false);

            // 计算电流路径的起始和结束点
            Vector3 positionA = towerA.transform.position;
            Vector3 positionB = towerB.transform.position;
//            Debug.Log($"Creating electric path between {towerA} and {towerB}");
            //实例化电流
            GameObject path = Instantiate(electricPathPrefab, (positionA + positionB) / 2, Quaternion.identity);
            //缩放
            Vector2 scale = path.transform.localScale;
            scale.x = Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.y - positionB.y)-1f;
            path.transform.localScale = scale;
            // 计算电流路径的朝向
            float angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
            path.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // 创建电流路径的Collider
            //BoxCollider2D collider = path.GetComponent<BoxCollider2D>();

            path.GetComponent<ElectricPath>().SetTowers(towerA, towerB, this);

            electricPaths.Add(path.GetComponent<ElectricPath>());
            /*
            if (collider != null)
            {
                collider.size = new Vector2(Mathf.Abs(positionA.x - positionB.x), Mathf.Abs(positionA.y - positionB.y));
                collider.isTrigger = true;
            */
                // 为塔设置电流路径
                //towerA.electricPaths.Add(path);
                //towerB.electricPaths.Add(path);

                // 激活电流路径
                //towerA.SetElectricPathActive(true);
                //towerB.SetElectricPathActive(true);

            //towerA.electricPathObject = path;
            //towerB.electricPathObject = path;
           // }
    }

    private bool IsElectricityAlreadyExists(Tower2 tower1, Tower2 tower2)
    {
        foreach (var electricity in electricPaths)
        {
            if ((electricity.towerA == tower1 && electricity.towerB == tower2) ||
                (electricity.towerA == tower2 && electricity.towerB == tower1))
            {
                return true;  // 已经存在电流连接
            }
        }
        return false;
    }
}
