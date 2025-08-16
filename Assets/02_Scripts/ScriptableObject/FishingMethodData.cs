using UnityEngine;

[CreateAssetMenu(fileName = "FishingMethodData", menuName = "FishingMethod/FishingMethodData")]
public class FishingMethodData : ScriptableObject
{
    public FishingStyle style;

    [SerializeField] float minBiteWaitTime;
    [SerializeField] float maxBiteWaitTime;

    public int maxHp;
    public int maxSp;
    public float maxCp;
    public float lowHpCpMultiplier;

    public float GetBiteWaitTime() => Random.Range(minBiteWaitTime, maxBiteWaitTime);
    public FishingStyle GetFishingStyle() => style;
}
