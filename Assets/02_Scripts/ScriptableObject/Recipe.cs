using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] List<RecipeEntry> recipeEntries;
    public List<RecipeEntry> RecipeEntries => recipeEntries;

    public bool IsCraftable()
    {
        Dictionary<int, Item> inventory = PlayerInventory.Instance.ItemDictionary;
        bool ret = true;

        for (int i = 0; i < recipeEntries.Count; i++)
        {
            if (inventory.TryGetValue(recipeEntries[i].item.itemId, out Item item)) 
            {
                if (item.ItemCount < recipeEntries[i].count) { ret = false; break; } 
            }
            else { ret = false; break; }
        }
        return ret;
    }
    public override string ToString()
    {
        string str = "";

        for (int i = 0; i < recipeEntries.Count; i++)
        {
            str += recipeEntries[i].item.itemName + " - " + recipeEntries[i].count.ToString();

            if (i != recipeEntries.Count - 1) str += '\n';
        }

        return str;
    }
}

[Serializable]
public class RecipeEntry
{
    public Item item;
    public int count;
}