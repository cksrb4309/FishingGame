using Npc;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "DialogueCondition/QuestInProgressCondition")]
    public class QuestInProgressCondition : DialogueCondition
    {
        public int questId;

        public override bool IsMet(NpcObj npc)
        {
            return Quest.QuestManager.Instance.IsQuestInProgress(questId);
        }
    }
}