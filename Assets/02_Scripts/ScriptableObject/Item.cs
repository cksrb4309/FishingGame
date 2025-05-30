using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    public string itemName;

    public FishingMethodData fishingMethodData;

    [SerializeField] BaitLootTable baitLootTable;

    public Item SelectItem(FishingZone zone) => baitLootTable.SelectItem(zone);
    public float GetBiteWaitTime() => fishingMethodData.GetBiteWaitTime();
    public FishingStyle GetFishingStyle() => fishingMethodData.GetFishingStyle();
}
