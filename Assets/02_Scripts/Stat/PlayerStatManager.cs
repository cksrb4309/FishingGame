using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    PlayerStat baseStat = new();
    PlayerStat equipmentStat = new();

    void Awake()
    {
        SetStat();
    }
    void SetStat()
    {
        baseStat.DefaultSetting();
        equipmentStat.Clear();
        PlayerStat.Stat.Apply(baseStat, equipmentStat);
    }
}
