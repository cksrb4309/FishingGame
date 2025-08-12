using Npc;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueConditionSet
    {
        public DialogueCondition rootCondition;  // 복합 조건 포함
        public DialogueTree dialogueTree;
        public int priority;

        public bool AreConditionsMet(NpcObj npc)
        {
            return rootCondition == null || rootCondition.IsMet(npc);
        }
    }
}