using System;
using UnityEngine;

public class FishingData : MonoBehaviour
{
    private static FishingData instance = null;

    private void Awake() => instance = this;

    int fishingLevel = 1;
    public static int FishingLevel => instance.fishingLevel;
    public static void SetFishingLevel(int level) => instance.fishingLevel = level;

    Item targetItem = null;
    public static Item TargetItem => instance.targetItem;
    public static void SetTargetItem(Item targetItem) => instance.targetItem = targetItem;
    public static FishingMethodData MethodData => instance.targetItem.fishingMethodData;

    [SerializeField] MiniGame_1_Data miniGame_1_Data;
    public static MiniGame_1_Data MiniGame_1_Data => instance.miniGame_1_Data;
}

[Serializable]
public class MiniGame_1_Data
{
    [SerializeField] int attackDamage;
    [SerializeField] float attackDelay;
    [SerializeField] float attackRange;
    [SerializeField] float attackInterval;

    [SerializeField] float damageMultiplier = 0.2f;
    [SerializeField] float delayMultiplier = 0.1f;
    [SerializeField] float rangeMultiplier = 0.2f;
    [SerializeField] float intervalMultiplier = 0.1f;

    public int AttackDamage { get => attackDamage + (int)(attackDamage * (FishingData.FishingLevel - 1) * damageMultiplier); }
    public float AttackDelay { get => attackDelay - attackDelay * (FishingData.FishingLevel - 1) * delayMultiplier; }
    public float AttackRange { get => attackRange + attackRange * (FishingData.FishingLevel - 1) * rangeMultiplier; }
    public float AttackInterval { get => attackInterval - attackInterval * (FishingData.FishingLevel - 1) * intervalMultiplier; }
}