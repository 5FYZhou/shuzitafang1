using UnityEngine;

public interface IHealthAccessor
{
    float[] HP { get; }
}
public interface enemy
{
    void attack(float DPH);
    void generate(int x, int y, int[,] new_waypint);
    void Start();
}

public interface IStrengthenTowerAttackPower
{
    void Strengthen(float per);
    void Reduce(float per);
}