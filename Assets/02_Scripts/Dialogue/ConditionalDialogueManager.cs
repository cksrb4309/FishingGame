using Npc;
using Quest;
using System.Collections.Generic;

namespace Dialogue
{
    [System.Serializable]
    public class ConditionalDialogueManager
    {
        public List<DialogueConditionSet> conditionSets;

        public DialogueTree GetAppropriateDialogue(NpcObj npc)
        {
            if (QuestManager.Instance.HasAvailableQuestFromNpc(npc.GetNpcName(), out var questDialogue))
            
                return questDialogue;
            
            DialogueConditionSet selectedSet = null;

            int highestPriority = int.MinValue;

            foreach (var conditionSet in conditionSets)
            {
                if (conditionSet.AreConditionsMet(npc) && conditionSet.priority > highestPriority)
                {
                    selectedSet = conditionSet;

                    highestPriority = conditionSet.priority;
                }
            }

            return selectedSet?.dialogueTree;
        }
    }
}
