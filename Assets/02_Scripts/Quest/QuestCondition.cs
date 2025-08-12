using UnityEngine;

namespace Quest
{
    public abstract class QuestCondition : ScriptableObject
    {
        public QuestConditionType conditionType;
        public abstract bool IsMet();
        public abstract void Register(QuestInfo questInfo);
    }
}
