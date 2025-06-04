using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; } = null;

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
                itemSlots[i].Setting(items[i]);

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
    public void GetItem(Item item)
    {
        item.itemCount++;

        if (!itemDictionary.ContainsKey(item.itemId)) itemDictionary[item.itemId] = item;
        
        SelectCategory(currentCategory);
    }
    public void UseItem(Item item, int usedCount = 1)
    {
        itemDictionary[item.itemId].itemCount -= usedCount;

        SelectCategory(currentCategory);
    }
    public void SelectBaitItem(Item item)
    {
        if (baitItem != null)
        {
            baitItem.isUsedAsBait = false;

            itemSlotDictionary[baitItem.itemId].Setting(baitItem);
        }

        if (item != null)
        {
            item.isUsedAsBait = true;

            itemSlotDictionary[item.itemId].Setting(item);
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
            itemSlotDictionary[baitItem.itemId].Setting(baitItem);
        }
    }
    public void SelectCategoryMaterial() => SelectCategory(ItemCategory.Material);
    public void SelectCategoryFunctional() => SelectCategory(ItemCategory.Functional);
    public void SelectCategoryEvent() => SelectCategory(ItemCategory.Event);
    public void SelectCategoryTool() => SelectCategory(ItemCategory.Tool);
}
