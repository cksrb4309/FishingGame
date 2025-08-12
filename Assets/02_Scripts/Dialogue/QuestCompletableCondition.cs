using Quest;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "DialogueCondition/QuestCompletableCondition")]
    public class QuestCompletableCondition : DialogueCondition
    {
        public QuestInfo questInfo;
        public int priority;
        public override bool IsMet(Npc.NpcObj npc)
        {
            if (questInfo.IsCompletable())
            {
                if (QuestManager.questPriority < priority)
                {
                    QuestManager.completeQuest = questInfo;
                    QuestManager.questPriority = priority;

                    return true;
                }
            }
            return false;
        }
    }
}

