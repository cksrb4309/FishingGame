using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public ItemCategory category;
    public int itemId;
    public int itemLevel = 1;
    public string itemName;
    public string itemExplain;
    public Sprite itemIcon;
    public FishingMethodData fishingMethodData;
    public BaitLootTable baitLootTable;

    [NonSerialized] public int itemCount = 0;
    [NonSerialized] public bool isUsedAsBait = false;
    public Item SelectItem(FishingZone zone, int fishingLevel) => baitLootTable.SelectItem(zone, fishingLevel);
    public float GetBiteWaitTime() => fishingMethodData.GetBiteWaitTime();
    public FishingStyle GetFishingStyle() => fishingMethodData.GetFishingStyle();
}
