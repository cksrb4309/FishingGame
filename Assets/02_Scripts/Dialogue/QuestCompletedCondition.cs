using Npc;

namespace Dialogue
{
    public class QuestCompletedCondition : DialogueCondition
    {
        public int questId;
        public override bool IsMet(NpcObj npc)
        {
            return Quest.QuestManager.Instance.IsQuestCompleted(questId);
        }
    }
}