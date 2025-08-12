using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "PreviousQuestCompletedCondition", menuName = "Quest/Condition/PreviousQuestCompleted")]
    public class PreviousQuestCompletedCondition : QuestCondition
    {
        public QuestInfo previousQuest;
        public override bool IsMet()
        {
            return previousQuest.isComplete;
        }
        public override void Register(QuestInfo questInfo)
        {
            QuestUnlockConditionManager.Instance.RegisterQuestCompleteCondition(previousQuest.questId, questInfo);
        }
    }
}