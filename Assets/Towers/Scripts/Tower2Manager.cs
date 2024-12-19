using System.Collections.Generic;
using UnityEngine;

public class Tower2Manager : MonoBehaviour
{
    private List<Tower2> towers = new List<Tower2>();  // �洢���е���
    public List<ElectricPath> electricPaths = new List<ElectricPath>();
    public GameObject electricPathPrefab;  // ����·����Ԥ�Ƽ�

    public Sprite electricSprite;

    //private void Start()
    //{
    // ��ʼ��ʱ������������
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

    // ����Ƿ�����������������
    private void CheckElectricCurrentBetweenTowers()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = i + 1; j < towers.Count; j++)
            {
                Tower2 towerA = towers[i];
                Tower2 towerB = towers[j];
                ElectricPath electricPath = IsElectricityAlreadyExists(towerA, towerB);
                if (towerA.CanAlignedWith(towerB) && !electricPath)  // �������ͬһˮƽ��ֱ���ϣ�����·����ͨ +û�е���
                {
                    // ��������·��
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

    // ��������·����Collider
    private void CreateElectricPathCollider(Tower2 towerA, Tower2 towerB)
    {
        // �������·������ʼ�ͽ�����
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
        //ʵ��������
        GameObject path = Instantiate(electricPathPrefab, (positionA + positionB) / 2, Quaternion.identity);

        //����
        Vector2 scale = path.transform.localScale;
        scale.x = Mathf.Abs(positionA.x - positionB.x) + Mathf.Abs(positionA.y - positionB.y) - 1f;
        path.transform.localScale = scale;

        // �������·���ĳ���
        float angle = Mathf.Atan2(positionB.y - positionA.y, positionB.x - positionA.x) * Mathf.Rad2Deg;
        path.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ��������·����Collider
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
                return electricity;  // �Ѿ����ڵ�������
            }
        }
        return null;
    }
}
