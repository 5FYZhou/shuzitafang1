using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour
{
    public GameObject[] monsterPrefabs; //�洢����Ԥ����
    public GameObject spawnPoint;       //���ɹ����λ��
    public float spawnInterval;         //���ɹ���ļ��
    private float timeSinceLastSpawn;    //�ϴ����ɹ����ʱ��
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void SpawnMonster()
    {
        GameObject enemyInstance = Instantiate(monsterPrefabs[0]);
        enemy script__ = enemyInstance.GetComponent<enemy>();
        script__.generate(0, 1, new int[,] { { 0, 0 }, { 6, 0 }, { 6, 9 }, { 11, 9 } });
    }
    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.currentGameState == GameState.Playing)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= spawnInterval)
            {
                SpawnMonster();
                timeSinceLastSpawn = 0;
            }
        }
    }
}
