using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_2_1 : MonoBehaviour, IHealthAccessor, enemy
{
    private int[,] new_waypoint;//整除l_of_side后的，*l_of_side后的点为对应的网格的左下角，对应map中的点，不是(没颠倒过的实际的地图)
    private bool is_attack;
    private int road, wall;
    private float attack_interval_counter;
    private Tower TowerScript;
    private Queue<Collider2D> triggerQueue = new();

    public float error_range, speed, DPH, l_of_side;
    public int map_height, map_width, point_counter;
    public int[] original_point, birth_point;//这是实际的像素点位，不是整除后的;
    public int[,] map, waypoint;//要跟实际的上下颠倒一下
    public float[] attack_interval, fission_range, HP;//HP:{now,all}
    public bool move,death;
    public Animator enemy_2_1_animation;

    float[] IHealthAccessor.HP
    {
        get { return HP; }
    }
    // Start is called before the first frame update
    public void Start()
    {
        enemy_2_1_animation = GetComponent<Animator>();

        //birth_point = new int[] { 0, 3 };
        original_point = new int[] { -5, 0 };//坐标原点，实际的像素点位

        waypoint = new int[1, 2];

        waypoint[0, 0] = 20;
        waypoint[0, 1] = 18;

        point_counter = 0;

        speed = 2*4f;

        error_range = 0.3f;

        move = true;
        death = false;
        l_of_side = 2;
        road = 8;
        wall = 0;
        map = new int[,] { { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0 },
                           { 2, 2, 2, 2, 6, 6, 2, 2, 2, 2, 8, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0 },
                           { 2, 2, 2, 2, 6, 6, 2, 2, 2, 2, 8, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0 },
                           { 2, 2, 2, 2, 6, 6, 2, 2, 2, 2, 8, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0 },
                           { 2, 2, 2, 2, 6, 6, 2, 2, 2, 2, 8, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 7, 7, 7 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 7, 7, 7 },
                           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7 } };
        map_height = map.GetLength(0);
        map_width = map.GetLength(1);

        HP = new float[] { 10f, 10f };

        attack_interval = new float[] { 0.25f, 0.5f, 0 };

        DPH = 5;

        attack_interval_counter = 0;

        is_attack = false;


        //无需改动
        //transform.position = new Vector3(birth_point[0] * l_of_side + original_point[0], birth_point[1] * l_of_side + original_point[1], 0f);
        waypoint = search_road(map);
        if (waypoint == null)
        {
            move = false;
        }

        enemy_2_1_animation.SetBool("move", move);
        enemy_2_1_animation.SetBool("is_attack", is_attack);
        enemy_2_1_animation.SetBool("death", death);
    }

    // Update is called once per frame
    void Update()
    {
        enemy_2_1_animation.SetBool("move", move);
        enemy_2_1_animation.SetBool("is_attack", is_attack);

        if (move && death == false)
        {
            float actual_x = (float)(waypoint[point_counter, 0] * l_of_side + original_point[0]);
            float actual_y = (float)(waypoint[point_counter, 1] * l_of_side + original_point[1]);

            int x_judge = range_judge(actual_x, transform.position.x, error_range * l_of_side);
            int y_judge = range_judge(actual_y, transform.position.y, error_range * l_of_side);

            if (x_judge == 0 && y_judge == 0)
            {
                if (point_counter < waypoint.GetLength(0) - 1)
                {
                    point_counter++;
                    waypoint = search_road(map);
                    if (waypoint == null)
                    {
                        move = false;
                    }
                }
                else
                {
                    move = false;
                }
            }
            else
            {
                if (x_judge != 0 && y_judge != 0)
                {
                    waypoint = search_road(map);
                    if (waypoint == null)
                    {
                        move = false;
                    }
                }
                else
                {
                    if (x_judge != 0)
                    {
                        transform.localScale = new Vector3(x_judge, transform.localScale.y, transform.localScale.z);
                    }
                    if (x_judge != 0)
                    {
                        float y_offset = 0f;
                        if (transform.position.y < actual_y - speed * Time.deltaTime)
                        {
                            y_offset = speed * Time.deltaTime;
                        }
                        else if (transform.position.y > actual_y + speed * Time.deltaTime)
                        {
                            y_offset = -speed * Time.deltaTime;
                        }
                        transform.position += new Vector3(speed * Time.deltaTime * x_judge, y_offset, 0);
                    }
                    if (y_judge != 0)
                    {
                        float x_offset = 0f;
                        if (transform.position.x < actual_x - speed * Time.deltaTime)
                        {
                            x_offset = speed * Time.deltaTime;
                        }
                        else if (transform.position.x > actual_x + speed * Time.deltaTime)
                        {
                            x_offset = -speed * Time.deltaTime;
                        }
                        transform.position += new Vector3(x_offset, speed * Time.deltaTime * y_judge, 0);
                    }
                }
            }
        }

        if (is_attack && death == false)
        {
            if (triggerQueue.Peek() == null && triggerQueue.Count - 1 > 0)
            {
                triggerQueue.Dequeue();
                TowerScript = triggerQueue.Peek().GetComponent<Tower>();
            }
            else if (triggerQueue.Peek() != null)
            {
                TowerScript = triggerQueue.Peek().GetComponent<Tower>();
            }
            if (TowerScript != null)
            {
                if (TowerScript.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                attack_interval_counter += Time.deltaTime;
                if (attack_interval[2] == 0 && attack_interval_counter >= attack_interval[0])
                {
                    TowerScript.TakeDamage(DPH);
                    attack_interval[2] = 1;
                }
                else if (attack_interval[2] == 1 && attack_interval_counter >= attack_interval[1])
                {
                    attack_interval[2] = 0;
                    attack_interval_counter = 0;
                }
            }
        }
    }

    public void generate(int x, int y, int[,] new_waypint)
    {
        transform.position = new Vector3(x * l_of_side + original_point[0], y * l_of_side + original_point[1], 0f);
        waypoint = deep_copy_two_d(new_waypint);
        Start();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("tower"))
        {
            triggerQueue.Enqueue(other);
            is_attack = true;
            attack_interval[2] = 0;
            move = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("tower"))
        {
            triggerQueue = queue_pop(triggerQueue, other);
            if (triggerQueue.Count == 0 && death == false)
            {
                move = true;
                is_attack = false;
                attack_interval[2] = 0;
            }
        }
    }

    int range_judge(float t, float x,float offset)
    {
        if (t + offset <= x)
        {
            return -1;
        }
        if (t - offset >= x)
        {
            return 1;
        }
        return 0;
    }
    int[,] search_road(int[,] map)
    {
        int self_x = (int)((transform.position.x + l_of_side / 2 - original_point[0]) / l_of_side);
        int self_y = (int)((transform.position.y + l_of_side / 2 - original_point[1]) / l_of_side);

        int[,] output = BFS(map, new int[] { waypoint[point_counter, 0], waypoint[point_counter, 1] }, self_x, self_y);
        if (output != null)
        {
            int[,] find_waypoint = re_output(output, output[output.GetLength(0) - 1, 2]);

            int w_l = waypoint.GetLength(0);
            int f_l = find_waypoint.GetLength(0);
            int i = 0, j = 0;
            new_waypoint = new int[w_l + f_l - 2, 2];
            for (i = 0; i < point_counter; i++)
            {
                for (j = 0; j < 2; j++)
                {
                    new_waypoint[i, j] = waypoint[i, j];
                }
            }
            i++;
            for (; i < point_counter + f_l; i++)
            {
                for (j = 0; j < 2; j++)
                {
                    new_waypoint[i - 1, j] = find_waypoint[i - point_counter, j];
                }
            }
            i++;
            for (; i < w_l + f_l; i++)
            {
                for (j = 0; j < 2; j++)
                {
                    new_waypoint[i - 2, j] = waypoint[i - f_l, j];
                }
            }
            return new_waypoint;
        }
        else
        {
            return null;
        }
    }
    int[,] BFS(int[,] map, int[] t_point, int x, int y)
    {
        bool find = false;
        //map中road表示路，wall表示不可进入
        int[,] queue = new int[map_height * map_width + 2, 3];

        int i = 0, j = 0;//i:queue遍历;j:queue尾

        int[,] map_ = deep_copy_two_d(map);

        queue[0, 0] = x;
        queue[0, 1] = y;
        queue[0, 2] = -1;
        j++;
        while (i != j)
        {
            x = queue[i, 0];
            y = queue[i, 1];
            if (x == t_point[0] && y == t_point[1])
            {
                find = true;
                break;
            }
            //w
            if (y > 0 && map_[y - 1, x] == road)
            {
                map_[y - 1, x] = wall;
                queue[j, 0] = x;
                queue[j, 1] = y - 1;
                queue[j, 2] = i;
                j++;
            }
            //d
            if (x < map_width - 1 && map_[y, x + 1] == road)
            {
                map_[y, x + 1] = wall;
                queue[j, 0] = x + 1;
                queue[j, 1] = y;
                queue[j, 2] = i;
                j++;
            }
            //s
            if (y < map_height - 1 && map_[y + 1, x] == road)
            {
                map_[y + 1, x] = wall;
                queue[j, 0] = x;
                queue[j, 1] = y + 1;
                queue[j, 2] = i;
                j++;
            }
            //a
            if (x > 0 && map_[y, x - 1] == road)
            {
                map_[y, x - 1] = wall;
                queue[j, 0] = x - 1;
                queue[j, 1] = y;
                queue[j, 2] = i;
                j++;
            }
            i++;
        }
        queue[queue.GetLength(0) - 1, 2] = i;
        if (find)
        {
            return queue;
        }
        else
        {
            return null;
        }
    }
    int[,] re_output(int[,] q, int i)
    {
        if (q[i, 2] == -1)
        {
            return new int[1, 2] { { q[i, 0], q[i, 1] } };
        }
        int[,] re__ = re_output(q, q[i, 2]);
        int re__l = re__.GetLength(0);
        if (re__l > 1 && ((q[i, 0] == re__[re__l - 1, 0] && re__[re__l - 1, 0] == re__[re__l - 2, 0]) || (q[i, 1] == re__[re__l - 1, 1] && re__[re__l - 1, 1] == re__[re__l - 2, 1])))
        {
            re__[re__l - 1, 0] = q[i, 0];
            re__[re__l - 1, 1] = q[i, 1];
            return re__;
        }

        return combine(re__, new int[1, 2] { { q[i, 0], q[i, 1] } });
    }
    int[,] combine(int[,] a, int[,] b)
    {
        int i = 0, j = 0;
        int height1 = a.GetLength(0);
        int width1 = a.GetLength(1);
        int height2 = b.GetLength(0) + height1;
        int[,] arry = new int[height2, width1];
        for (i = 0; i < height1; i++)
        {
            for (j = 0; j < width1; j++)
            {
                arry[i, j] = a[i, j];
            }
        }
        for (; i < height2; i++)
        {
            for (j = 0; j < width1; j++)
            {
                arry[i, j] = b[i - height1, j];
            }
        }
        return arry;
    }
    int[,] deep_copy_two_d(int[,] a)
    {
        int height = a.GetLength(0);
        int width = a.GetLength(1);
        int i, j;
        int[,] b = new int[height, width];
        for (i = 0; i < height; i++)
        {
            for (j = 0; j < width; j++)
            {
                b[i, j] = a[i, j];
            }
        }
        return b;
    }
    public void attack(float ATK)
    {
        HP[0] = Mathf.Max(HP[0] - ATK, 0);
        if (HP[0] <= 0)
        {
            move = false;
            death = true;
            enemy_2_1_animation.SetBool("death", death);
            StartCoroutine(DeadDelay(1f));
        }
    }

    IEnumerator DeadDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);//dead
    }

    Queue<Collider2D> queue_pop(Queue<Collider2D> queue, Collider2D target)
    {
        if (queue.Peek() == target)
        {
            queue.Dequeue();
            return queue;
        }
        Queue<Collider2D> queue_new = new Queue<Collider2D>();
        while (queue.Count > 0)
        {
            if (queue.Peek() != target)
            {
                queue_new.Enqueue(queue.Dequeue());
            }
        }
        return queue_new;
    }

    void outp(int[,] a)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            Debug.Log("x" + i + ":" + a[i, 0]);
            Debug.Log("y" + i + ":" + a[i, 1]);
        }
    }
}
