using System;

[Serializable]
public class QuestItem
{
    public Item targetItem;
    public int itemCount;

    public bool HasEnoughItems() => targetItem.ItemCount >= itemCount;
}