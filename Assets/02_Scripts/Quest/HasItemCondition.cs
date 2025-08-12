using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "HasItemCondition", menuName = "Quest/Condition/HasItem")]
    public class HasItemCondition : QuestCondition
    {
        public Item requiredItem;
        public override bool IsMet()
        {
            return requiredItem.ItemCount > 0;
        }
        public override void Register(QuestInfo questInfo)
        {
            QuestUnlockConditionManager.Instance.RegisterItemCondition(requiredItem, questInfo);
        }
    }
}