using UnityEngine;

public interface IHealthAccessor
{
    float[] HP { get; }
}
public interface enemy
{
    void attack(float DPH);
    void generate(int x, int y, int[,] new_waypint,enemy_generator script);
    void Start();

    bool Death { get; set; }
}