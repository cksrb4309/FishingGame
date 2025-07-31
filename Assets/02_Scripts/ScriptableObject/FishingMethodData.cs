using UnityEngine;

[CreateAssetMenu(fileName = "FishingMethodData", menuName = "FishingMethod/FishingMethodData")]
public class FishingMethodData : ScriptableObject
{
    public FishingStyle style;
    [SerializeField] float minBiteWaitTime;
    [SerializeField] float maxBiteWaitTime;

    public float GetBiteWaitTime() => Random.Range(minBiteWaitTime, maxBiteWaitTime);
    public FishingStyle GetFishingStyle() => style;
}
