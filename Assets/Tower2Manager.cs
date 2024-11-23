using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower2Manager : MonoBehaviour
{
    private List<Tower2> towers = new List<Tower2>();  // 存储所有的塔
    public GameObject electricPathPrefab;  // 电流路径的预制件

    private void Start()
    {
        // 初始化时，查找所有塔
        towers.AddRange(FindObjectsOfType<Tower2>());
        Debug.Log(towers.Count);
    }

    private void Update()
    {
        // 每帧检查塔之间的电流
        CheckElectricCurrentBetweenTowers();
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
                if (towerA.IsAlignedWith(towerB) && towerA.IsPathClear(towerB))  // 如果塔在同一水平或垂直线上，并且路径畅通
                {
                    // 创建电流路径
                    Debug.Log("creat");
                    CreateElectricPathCollider(towerA, towerB);
                }
                else
                {
                    // 禁用电流路径
                    towerA.SetElectricPathActive(false);
                    towerB.SetElectricPathActive(false);
                }
            }
        }
    }

    // 创建电流路径的Collider
    private void CreateElectricPathCollider(Tower2 towerA, Tower2 towerB)
    {
        // 确保只有一条电流路径存在
        if (towerA.electricPathCollider == null || towerB.electricPathCollider == null)
        {
            // 先禁用所有路径
            towerA.SetElectricPathActive(false);
            towerB.SetElectricPathActive(false);

            // 计算电流路径的起始和结束点
            Vector3 positionA = towerA.transform.position;
            Vector3 positionB = towerB.transform.position;
            Debug.Log($"Creating electric path between {positionA} and {positionB}");
            // 创建电流路径的Collider
            GameObject path = Instantiate(electricPathPrefab, (positionA + positionB) / 2, Quaternion.identity);
            BoxCollider2D collider = path.GetComponent<BoxCollider2D>();

            if (collider != null)
            {
                collider.size = new Vector2(Mathf.Abs(positionA.x - positionB.x), Mathf.Abs(positionA.y - positionB.y));
                collider.isTrigger = true;

                // 计算电流路径的朝向
                float angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
                path.transform.rotation = Quaternion.Euler(0, 0, angle);

                // 为塔设置电流路径
                towerA.electricPathCollider = collider;
                towerB.electricPathCollider = collider;

                // 激活电流路径
                towerA.SetElectricPathActive(true);
                towerB.SetElectricPathActive(true);
            }
        }
    }
}
