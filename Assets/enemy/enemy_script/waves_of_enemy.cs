using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class waves_of_enemy : MonoBehaviour
{
    public enemy_generator enemy_Generator;
    public TextMeshProUGUI wavetext;
    // Start is called before the first frame update
    void Start()
    {
        enemy_Generator= FindObjectOfType<enemy_generator>();
        wavetext.text = enemy_Generator.wave_num[0] + "/" + (enemy_Generator.wave_num.GetLength(0) - 1);
    }

    // Update is called once per frame
    void Update()
    {
        wavetext.text = enemy_Generator.wave_num[0] + "/" + (enemy_Generator.wave_num.GetLength(0) - 1);
        if (enemy_Generator != null)
        {
            Vector2 scale = transform.localScale;
            if (enemy_Generator.time_counter[0] / 60 < enemy_Generator.wave_num.GetLength(0)-2)
            {
                scale.x = (enemy_Generator.time_counter[0] % 60) / 60;
                transform.localScale = scale;
            }
            else
            {
                scale.x = 1;
                transform.localScale = scale;
            }
        }
        else
        {
            enemy_Generator = FindObjectOfType<enemy_generator>();
        }
    }
}
