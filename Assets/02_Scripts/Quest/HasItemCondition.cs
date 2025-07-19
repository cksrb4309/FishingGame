using UnityEngine;

[System.Serializable]
public class HasItemCondition : QuestCondition
{
    [SerializeField] Item requiredItem;
    public override bool IsMet()
    {
        return requiredItem.itemCount > 0;
    }
}