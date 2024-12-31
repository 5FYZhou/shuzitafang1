using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_generator : MonoBehaviour
{
    public GameObject[] enemy_list;
    public GameObject enemyInstance;
    public float[] time_counter = new float[] { 0, 0 };//{time,turn}
    public float[] wave_num = new float[] { 0, 0, 60, 120, 180, 240 };//turn,each_time
    public int[] enemy_kill_counter = new int[] { 0, 0 };
    public float[,] gen_list;

    private bool can_function = true;
    private enemy enemy_script;

    // 存储待生成敌人信息的列表
    private List<(int type, int num)> enemiesToSpawn = new List<(int type, int num)>();

    // Start is called before the first frame update
    void Start()
    {
        int i;
        gen_list = new float[,] { { 0, 1, 1 }, { 10, 1, 1 }, { 20, 1, 2 }, { 30, 1, 2 }, { 40, 1, 3 }, { 60, 1, 2 }, { 70, 1, 2 }, { 80, 1, 3 }, { 90, 1, 4 }, { 100, 1, 5 }, { 120, 2, 2 }, { 130, 2, 2 }, { 140, 2, 3 }, { 180, 1, 8 }, { 190, 2, 4 }, { 240, 2, 2 }, { 250, 1, 2 }, { 260, 3, 1 } };
        enemy_kill_counter[1] = 0;
        for (i = 0; i < gen_list.GetLength(0); i++)
        {
            enemy_kill_counter[1] += (int)gen_list[i, 2];
        }
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_kill_counter[0] >= enemy_kill_counter[1])
        {
            GameStateManager.currentGameState = GameState.GameOver;
        }

        if (GameStateManager.currentGameState == GameState.Playing)
        {
            time_counter[0] += Time.deltaTime;
            if (wave_num[0]+1<wave_num.GetLength(0) && time_counter[0] >= wave_num[(int)wave_num[0]+1])
            {
                wave_num[0] += 1;
            }
            while (time_counter[1] < gen_list.GetLength(0) && time_counter[0] >= gen_list[(int)time_counter[1], 0])
            {
                int type = (int)gen_list[(int)time_counter[1], 1] - 1;
                int num = (int)gen_list[(int)time_counter[1], 2];
                enemiesToSpawn.Add((type, num));
                time_counter[1] += 1;
            }
        }
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (GameStateManager.currentGameState == GameState.Playing)
        {
            if (enemiesToSpawn.Count > 0)
            {
                var (type, num) = enemiesToSpawn[0];
                if (can_function)
                {
                    can_function = false;
                    enemyInstance = Instantiate(enemy_list[type]);
                    enemy_script = enemyInstance.GetComponent<enemy>();
                    enemy_script.generate(0, 1, new int[,] { { 0, 0 }, { 6, 0 }, { 6, 9 }, { 11, 9 } }, transform.GetComponent<enemy_generator>());
                    yield return new WaitForSeconds(1f);
                    num--;
                    if (num == 0)
                    {
                        enemiesToSpawn.RemoveAt(0);
                    }
                    else
                    {
                        enemiesToSpawn[0] = (type, num);
                    }
                    can_function = true;
                }
            }
            yield return null;
        }
    }
}