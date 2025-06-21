using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FishPattern", menuName = "FishPattern/FishPattern")]
public class FishPattern : ScriptableObject
{
    [SerializeField] FishPatternData[] fishPatternDatas;
    [SerializeField] int startPattern = 0;

    FishPatternData current = null;
}

[Serializable]
public class FishPatternData
{
    [SerializeField] FishPatternObj fishPatternObj;

    [SerializeField] int damage;

    [SerializeField] Vector2 pivotPosition;
    [SerializeField] float angle;

    [SerializeField] bool useRandomPosition;
    [SerializeField] bool useRandomAngle;

    [SerializeField] Vector2Int[] nextPatternProbability;

    public int NextPattern()
    {
        int totalWeight = nextPatternProbability.Sum(p => p.y);
        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var p in nextPatternProbability)
        {
            cumulative += p.y;

            if (randomValue < cumulative) return p.x;
        }
        return nextPatternProbability[nextPatternProbability.Length - 1].x;
    }
}