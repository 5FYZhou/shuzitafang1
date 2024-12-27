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
        int monsterIndex = Random.Range(0, monsterPrefabs.Length);
        GameObject monster = Instantiate(monsterPrefabs[monsterIndex], spawnPoint.transform.position, Quaternion.identity);
        monster.transform.SetParent(spawnPoint.transform);
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
