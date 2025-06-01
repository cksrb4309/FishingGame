using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public ItemCategory category;
    public int itemId;
    public string itemName;
    public Sprite itemIcon;
    public FishingMethodData fishingMethodData;

    [SerializeField] BaitLootTable baitLootTable;

    [NonSerialized] public int itemCount = 0;

    public Item SelectItem(FishingZone zone) => baitLootTable.SelectItem(zone);
    public float GetBiteWaitTime() => fishingMethodData.GetBiteWaitTime();
    public FishingStyle GetFishingStyle() => fishingMethodData.GetFishingStyle();
}
