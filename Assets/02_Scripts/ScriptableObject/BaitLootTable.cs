using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

[CreateAssetMenu(fileName = "BaitLootTable", menuName = "Fishing/BaitLootTable")]
public class BaitLootTable : ScriptableObject
{
    [SerializeField] List<BaitLootData> baitLootDatas;

    public Item SelectItem(FishingZone zone)
    {
        var filteredLoots = baitLootDatas.Where(wi => zone.items.Contains(wi.targetItem)).ToList();

        if (filteredLoots.Count == 0) return null;

        float totalWeight = filteredLoots.Sum(wi => wi.probability);
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0;

        foreach (var wi in filteredLoots)
        {
            cumulative += wi.probability;

            if (randomValue < cumulative) return wi.targetItem;
        }
        return null;
    }
}

[Serializable]
public class BaitLootData
{
    public Item targetItem;
    public float probability;
}