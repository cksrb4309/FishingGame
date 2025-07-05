using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; } = null;
    public Dictionary<int, Item> ItemDictionary => itemDictionary;
    [SerializeField] ItemSlot[] itemSlots;

    Dictionary<int, Item> itemDictionary = new();
    Dictionary<int, ItemSlot> itemSlotDictionary = new();

    ItemCategory currentCategory = ItemCategory.Material;

    Item baitItem = null;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SelectCategory(ItemCategory.Material);
    }
    void SelectCategory(ItemCategory category)
    {
        ExplainText.Instance.Setting(string.Empty);
        ContextMenuManager.Instance.CloseMenu();

        currentCategory = category;

        for (int i = 0; i < itemSlots.Length; i++) itemSlots[i].Clear();

        List<Item> items = SelectItemByCategory(category);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemCount > 0)
            {
                itemSlots[i].SettingItem(items[i]);

                itemSlotDictionary[items[i].itemId] = itemSlots[i];
            }
            else
            {
                items.RemoveAt(i);
                i--;
            }
        }
    }
    List<Item> SelectItemByCategory(ItemCategory category)
    {
        var filtered = (category == ItemCategory.All) ?
            itemDictionary.Values.Where(i => i.itemCount > 0) :
            itemDictionary.Values.Where(i => (i.category & category) != 0 && (i.itemCount > 0));

        return filtered.OrderBy(i => i.itemId).ToList();
    }
    public void GetItem() // ≥¨Ω√∏¶ ≈Î«ÿ æ∆¿Ã≈€ »πµÊ Ω√ »£√‚
    {
        GetItem(FishingData.TargetItem);
    }
    public void GetItem(Item item)
    {
        item.itemCount++;

        if (!itemDictionary.ContainsKey(item.itemId))
            itemDictionary[item.itemId] = item;

        QuestManager.Instance.CheckCompletableQuest(item);

        SelectCategory(currentCategory);
    }
    public void GetQuestReward(QuestInfo questInfo)
    {
        for (int i = 0; i < questInfo.rewardItems.Count; i++)
        {
            questInfo.rewardItems[i].targetItem.itemCount += questInfo.rewardItems[i].itemCount;

            if (!itemDictionary.ContainsKey(questInfo.rewardItems[i].targetItem.itemId))
                itemDictionary[questInfo.rewardItems[i].targetItem.itemId] = questInfo.rewardItems[i].targetItem;

            QuestManager.Instance.CheckCompletableQuest(questInfo.rewardItems[i].targetItem);
        }

        SelectCategory(currentCategory);
    }
    public void UseQuestItem(QuestInfo questInfo)
    {
        for (int i = 0; i < questInfo.questItems.Count; i++)
        {
            questInfo.questItems[i].targetItem.itemCount -= questInfo.questItems[i].itemCount;

            if (!itemDictionary.ContainsKey(questInfo.questItems[i].targetItem.itemId))
                itemDictionary[questInfo.questItems[i].targetItem.itemId] = questInfo.questItems[i].targetItem;

            QuestManager.Instance.CheckCompletableQuest(questInfo.questItems[i].targetItem);
        }

        SelectCategory(currentCategory);
    }
    public void UseItem(Item item, int usedCount = 1)
    {
        itemDictionary[item.itemId].itemCount -= usedCount;

        SelectCategory(currentCategory);
    }
    public void UseItem(Recipe recipe)
    {
        foreach (RecipeEntry entry in recipe.RecipeEntries)
        {
            itemDictionary[entry.item.itemId].itemCount -= entry.count;
        }
        SelectCategory(currentCategory);
    }
    public void SelectBaitItem(Item item)
    {
        if (baitItem != null)
        {
            baitItem.isUsedAsBait = false;

            itemSlotDictionary[baitItem.itemId].SettingItem(baitItem);
        }

        if (item != null)
        {
            item.isUsedAsBait = true;

            itemSlotDictionary[item.itemId].SettingItem(item);
        }
        else SelectCategory(currentCategory);

        PlayerFishingManager.Instance.SelectBait(item);

        baitItem = item;
    }
    public void UseBaitItem()
    {
        baitItem.itemCount--;

        if (baitItem.itemCount <= 0)
        {
            SelectBaitItem(null);

            PlayerFishingManager.Instance.SelectBait(null);
        }
        else
        {
            itemSlotDictionary[baitItem.itemId].SettingItem(baitItem);
        }
    }
    public void SelectCategoryMaterial() => SelectCategory(ItemCategory.Material);
    public void SelectCategoryFunctional() => SelectCategory(ItemCategory.Functional);
    public void SelectCategoryEvent() => SelectCategory(ItemCategory.Event);
    public void SelectCategoryTool() => SelectCategory(ItemCategory.Tool);
}
