using System.Collections.Generic;

public class PlayerStat
{
    public static PlayerStat Stat = new();

    Dictionary<StatType, int> stats_Int = new Dictionary<StatType, int>();
    Dictionary<StatType, float> stats_Float = new Dictionary<StatType, float>();

    public int projectileDamage = 0; // 투사체 데미지
    public int stunAccumulation = 0; // 기절 데미지
    public float stunDuration = 0; // 기절 시간
    public int catchGaugeGain = 0; // 낚기 데미지
    public float biteFrequency = 0; // 입질 빈도 (입질에 영향)
    public float highGradeFishChance = 0; // 높은 등급의 물고기 잡힐 확률 증가

    public float attackInterval = 0; // 미니게임1 어택 간격

    public void Apply(params PlayerStat[] stats)
    {
        Clear();

        foreach (PlayerStat stat in stats)
        {
            stats_Int[StatType.FirePower] += stat.stats_Int[StatType.FirePower];
            stats_Int[StatType.Strength] += stat.stats_Int[StatType.Strength];
            stats_Int[StatType.Luck] += stat.stats_Int[StatType.Luck];
        }

        SetDetail();
    }
    public void Clear()
    {
        stats_Int[StatType.FirePower] = 0;
        stats_Int[StatType.Strength] = 0;
        stats_Int[StatType.Luck] = 0;

        SetDetail();
    }
    public void ModifyStat(StatType statType, int amount)
    {
        stats_Int[statType] += amount;

        SetDetail();
    }
    public void ModifyStat(StatType statType, float amount)
    {
        stats_Float[statType] += amount;
        
        SetDetail();
    }
    void SetDetail()
    {
        projectileDamage = stats_Int[StatType.FirePower];
        stunAccumulation = stats_Int[StatType.FirePower];

        stunDuration = stats_Int[StatType.Strength];
        catchGaugeGain = stats_Int[StatType.Strength];

        biteFrequency = stats_Int[StatType.Luck];
        highGradeFishChance = stats_Int[StatType.Luck];
    }
    public void DefaultSetting()
    {
        stats_Int[StatType.FirePower] = 2;
        stats_Int[StatType.Strength] = 2;
        stats_Int[StatType.Luck] = 1;

        SetDetail();
    }
}

public enum StatType
{
    FirePower,
    Strength,
    Luck,
}