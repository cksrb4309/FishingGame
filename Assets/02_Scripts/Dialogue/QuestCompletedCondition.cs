using Npc;

namespace Dialogue
{
    public class QuestCompletedCondition : DialogueCondition
    {
        public Quest.QuestInfo quest;
        public override bool IsMet(NpcObj npc)
        {
            return Quest.QuestManager.Instance.IsQuestCompleted(quest.questId);
        }
    }
}