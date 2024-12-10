using System.Collections.Generic;
using UnityEngine;

public class Tower2Manager : MonoBehaviour
{
    private List<Tower2> towers = new List<Tower2>();  // 存储所有的塔
    public List<ElectricPath> electricPaths = new List<ElectricPath>();
    public GameObject electricPathPrefab;  // 电流路径的预制件

    public Sprite electricSprite;

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

    private void Remove(ElectricPath elecPath)
    {
        List<ElectricPath> elecPaths = new List<ElectricPath>();
        foreach (ElectricPath path in electricPaths)
        {
            if (path != elecPath)
            {
                elecPaths.Add(path);
            }
        }
        electricPaths = elecPaths;
    }

    public void DestroyElectricPath(ElectricPath electricPath)
    {
        Destroy(electricPath.gameObject);
        Remove(electricPath);
    }

    // 检查是否有两个塔产生电流
    private void CheckElectricCurrentBetweenTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = i + 1; j < towers.Count; j++)
            {
                Tower2 towerA = towers[i];
                Tower2 towerB = towers[j];
                ElectricPath electricPath = IsElectricityAlreadyExists(towerA, towerB);
                if (towerA.CanAlignedWith(towerB) && !electricPath)  // 如果塔在同一水平或垂直线上，并且路径畅通 +没有电流
                {
                    // 创建电流路径
                    //Debug.Log($"creat between {towerA} and {towerB}");
                    CreateElectricPathCollider(towerA, towerB);
                }
                else if ((!towerA.CanAlignedWith(towerB) && !towerB.CanAlignedWith(towerA)) && electricPath)
                {
                    DestroyElectricPath(electricPath);
                }
            }
        }
    }

    // 创建电流路径的Collider
    private void CreateElectricPathCollider(Tower2 towerA, Tower2 towerB)
    {
        // 计算电流路径的起始和结束点
        Vector3 positionA = towerA.transform.position;
        Vector3 positionB = towerB.transform.position;
        //Debug.Log($"Creating electric path between {towerA} and {towerB}");
        /*
        GameObject elecpath = new GameObject("electricPath");
        elecpath.transform.position = (positionA + positionB) / 2;
        BoxCollider2D elecBoxCollider = elecpath.AddComponent<BoxCollider2D>();
        elecBoxCollider.isTrigger = true;
        elecBoxCollider.size = new Vector2(Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.y - positionB.y) - 1f, 0.3f);
        float angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
        elecpath.transform.rotation = Quaternion.Euler(0, 0, angle);
        elecpath.AddComponent<ElectricPath>();
        */
        //实例化电流
        GameObject path = Instantiate(electricPathPrefab, (positionA + positionB) / 2, Quaternion.identity);

        //缩放
        Vector2 scale = path.transform.localScale;
        scale.x = Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.y - positionB.y) - 1f;
        path.transform.localScale = scale;

        // 计算电流路径的朝向
        /*float */angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
        path.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 创建电流路径的Collider
        //BoxCollider2D collider = path.GetComponent<BoxCollider2D>();

        path.GetComponent<ElectricPath>().SetTowers(towerA, towerB, this);

        electricPaths.Add(path.GetComponent<ElectricPath>());

    }

    private ElectricPath IsElectricityAlreadyExists(Tower2 tower1, Tower2 tower2)
    {
        foreach (var electricity in electricPaths)
        {
            if ((electricity.towerA == tower1 && electricity.towerB == tower2) ||
                (electricity.towerA == tower2 && electricity.towerB == tower1))
            {
                return electricity;  // 已经存在电流连接
            }
        }
        return null;
    }
}
