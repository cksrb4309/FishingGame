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

    ItemCategory currentCategory = ItemCategory.All;

    public void SelectCategory(ItemCategory category)
    {
        currentCategory = category;

        for (int i = 0; i < itemSlots.Length; i++) itemSlots[i].Clear();

        List<Item> items = SelectItemByCategory(category);

        for (int i = 0; i < items.Count; i++) itemSlots[i].Setting(items[i]);
    }
    List<Item> SelectItemByCategory(ItemCategory category)
    {
        //var filtered = (category == ItemCategory.All) ? allItems : allItems.Where(i => (i.category & category) != 0);

        //return filtered.OrderBy(i => i.itemId).ToList();
        var filtered = (category == ItemCategory.All) ? itemDictionary.Values.ToList() : itemDictionary.Values.Where(i => (i.category & category) != 0 && (i.itemCount > 0));

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
}