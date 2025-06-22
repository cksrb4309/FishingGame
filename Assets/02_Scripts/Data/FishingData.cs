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

    [SerializeField] MiniGame_2_Data miniGame_2_Data;
    public static MiniGame_2_Data MiniGame_2_Data => instance.miniGame_2_Data;
    [SerializeField] MiniGame_3_Data miniGame_3_Data;
    public static MiniGame_3_Data MiniGame_3_Data => instance.miniGame_3_Data;
    [SerializeField] MiniGame_4_Data miniGame_4_Data;
    public static MiniGame_4_Data MiniGame_4_Data => instance.miniGame_4_Data;
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
[Serializable]
public class MiniGame_2_Data
{
    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackInterval;

    [SerializeField] float damageMultiplier = 0.2f;
    [SerializeField] float rangeMultiplier = 0.2f;
    [SerializeField] float intervalMultiplier = 0.1f;

    public int AttackDamage { get => attackDamage + (int)(attackDamage * (FishingData.FishingLevel - 1) * damageMultiplier); }
    public float AttackRange { get => attackRange + attackRange * (FishingData.FishingLevel - 1) * rangeMultiplier; }
    public float AttackInterval { get => attackInterval - attackInterval * (FishingData.FishingLevel - 1) * intervalMultiplier; }
}

[Serializable]
public class MiniGame_3_Data
{
    [SerializeField] int attackDamage;
    [SerializeField] float attackInterval;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileSize;

    [SerializeField] float damageMultiplier = 0.2f;
    [SerializeField] float intervalMultiplier = 0.1f;
    [SerializeField] float projectileSpeedMultiplier = 0.2f;
    [SerializeField] float projectileSizeMultiplier = 0.1f;

    public int AttackDamage { get => attackDamage + (int)(attackDamage * (FishingData.FishingLevel - 1) * damageMultiplier); }
    public float AttackInterval { get => attackInterval - attackInterval * (FishingData.FishingLevel - 1) * intervalMultiplier; }
    public float ProjectileSpeed { get => projectileSpeed + projectileSpeed * (FishingData.FishingLevel - 1) * projectileSpeedMultiplier; }
    public float ProjectileSize { get => projectileSize + projectileSize * (FishingData.FishingLevel - 1) * projectileSizeMultiplier; }
}

[Serializable]
public class MiniGame_4_Data
{
    [SerializeField] int attackDamage;
    [SerializeField] int maxHp;
    [SerializeField] float moveSpeed;
    [SerializeField] float dashRegen;
    
    [SerializeField] float damageMultiplier = 0.2f;
    [SerializeField] float maxHpMultiplier = 0.2f;
    [SerializeField] float moveSpeedMultiplier = 0.2f;
    [SerializeField] float dashRegenMultiplier = 0.1f;

    public int AttackDamage { get => attackDamage + (int)(attackDamage * (FishingData.FishingLevel - 1) * damageMultiplier); }
    public int MaxHp { get => maxHp + (int)(maxHp * (FishingData.FishingLevel - 1) * maxHpMultiplier); }
    public float MoveSpeed { get => moveSpeed + moveSpeed * (FishingData.FishingLevel - 1) * moveSpeedMultiplier; }
    public float DashRegen { get => dashRegen + dashRegen * (FishingData.FishingLevel - 1) * dashRegenMultiplier; }
}