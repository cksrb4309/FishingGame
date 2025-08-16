using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using System;
using TMPro;
using Quest;
public class PlayerInventory : UIInputPanel
{
    public static PlayerInventory Instance { get; private set; } = null;
    public Dictionary<int, Item> ItemDictionary => itemDictionary;
    [SerializeField] ItemSlot[] itemSlots;

    [SerializeField] SerializedDictionary<InventoryState, GameObject> inventoryStateToPanel;

    Dictionary<int, Item> itemDictionary = new();
    Dictionary<int, ItemSlot> itemSlotDictionary = new();

    ItemCategory currentCategory = ItemCategory.Material;

    [NonSerialized] public InventoryState inventoryState = InventoryState.None;

    TMP_Text moneyText;

    Item baitItem = null;

    bool isShow = false;

    int money = 0;

    int Money
    {
        get => money;
        set
        {
            money = value;

            moneyText.text = money.ToString();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }
    private void Start()
    {
        moneyText = GameObject.Find("NumberText").GetComponent<TMP_Text>();

        SelectCategory(ItemCategory.Material);
    }
    public override void Show(bool trigger = true)
    {
        if (isShow) Hide();
        
        else
        {
            isShow = true;

            base.Show(trigger);

            OpenInventory(InventoryState.None);

            SelectCategory(currentCategory);
        }
    }
    public void ShowShop()
    {
        if (isShow)
        {
            HideShop();
        }
        else
        {
            isShow = true;

            base.Show(true);

            OpenInventory(InventoryState.Shop);

            SelectCategory(currentCategory);
        }
    }
    public override void Hide(bool trigger = true)
    {
        isShow = false;

        base.Hide(trigger);

        if (trigger) UIManager.OnHideAll();

        ContextMenuManager.Instance.CloseMenu();
    }
    public void HideShop()
    {
        if (isShow && inventoryState == InventoryState.Shop) Hide();
    }
    public void OpenInventory(InventoryState inventoryState)
    {
        this.inventoryState = inventoryState;

        foreach (var key in inventoryStateToPanel.Keys)

            inventoryStateToPanel[key].SetActive(key == inventoryState);
    }
    void SelectCategory(ItemCategory category)
    {
        ExplainText.Instance.Setting(null);

        ContextMenuManager.Instance.CloseMenu();

        currentCategory = category;

        for (int i = 0; i < itemSlots.Length; i++) itemSlots[i].Clear();

        List<Item> items = SelectItemByCategory(category);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ItemCount > 0)
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
            itemDictionary.Values.Where(i => i.ItemCount > 0) :
            itemDictionary.Values.Where(i => (i.category & category) != 0 && (i.ItemCount > 0));

        return filtered.OrderBy(i => i.itemId).ToList();
    }
    public void GetItem() // ≥¨Ω√∏¶ ≈Î«ÿ æ∆¿Ã≈€ »πµÊ Ω√ »£√‚
    {
        GetItem(FishingData.TargetItem);
    }
    public void GetItem(Item item)
    {
        item.ItemCount++;

        if (!itemDictionary.ContainsKey(item.itemId))
            itemDictionary[item.itemId] = item;

        QuestManager.Instance.CheckCompletableQuest(item);

        SelectCategory(currentCategory);
    }
    public void GetQuestReward(QuestInfo questInfo)
    {
        for (int i = 0; i < questInfo.rewardItems.Count; i++)
        {
            questInfo.rewardItems[i].targetItem.ItemCount += questInfo.rewardItems[i].itemCount;

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
            questInfo.questItems[i].targetItem.ItemCount -= questInfo.questItems[i].itemCount;

            if (!itemDictionary.ContainsKey(questInfo.questItems[i].targetItem.itemId))
                itemDictionary[questInfo.questItems[i].targetItem.itemId] = questInfo.questItems[i].targetItem;

            QuestManager.Instance.CheckCompletableQuest(questInfo.questItems[i].targetItem);
        }

        SelectCategory(currentCategory);
    }
    public void UseItem(Item item, int usedCount = 1)
    {
        itemDictionary[item.itemId].ItemCount -= usedCount;

        SelectCategory(currentCategory);
    }
    public void UseItem(Recipe recipe)
    {
        foreach (RecipeEntry entry in recipe.RecipeEntries)
        {
            itemDictionary[entry.item.itemId].ItemCount -= entry.count;
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
        baitItem.ItemCount--;

        if (baitItem.ItemCount <= 0)
        {
            SelectBaitItem(null);

            PlayerFishingManager.Instance.SelectBait(null);
        }
        else
        {
            itemSlotDictionary[baitItem.itemId].SettingItem(baitItem);
        }
    }
    public void SellItem(Item item)
    {
        Money += item.itemPrice;

        item.ItemCount--;

        if (item.ItemCount <= 0) ShopExplainText.Instance.Setting(null);
        
        SelectCategory(currentCategory);
    }
    public void SelectCategoryMaterial() => SelectCategory(ItemCategory.Material);
    public void SelectCategoryFunctional() => SelectCategory(ItemCategory.Functional);
    public void SelectCategoryEvent() => SelectCategory(ItemCategory.Event);
    public void SelectCategoryTool() => SelectCategory(ItemCategory.Tool);
}

public enum InventoryState
{
    None,
    Shop,
}