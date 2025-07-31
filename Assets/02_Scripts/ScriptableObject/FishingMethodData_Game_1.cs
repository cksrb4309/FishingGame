using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingMethodData_Game_1", menuName = "FishingMethod/FishingMethodData_Game_1")]
public class FishingMethodData_Game_1 : FishingMethodData
{
    public int maxHp;
    public int maxSp;
    public float maxCp;

    public float speed;
    public float lowHpCpMultiplier;

    public float minRotateDelay;
    public float maxRotateDelay;
    public float balanceRotateDelay;
}
