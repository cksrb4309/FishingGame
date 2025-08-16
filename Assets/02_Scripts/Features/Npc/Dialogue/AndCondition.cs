using Npc;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "DialogueCondition/AndCondition")]
    public class AndCondition : DialogueCondition
    {
        public List<DialogueCondition> subConditions;

        public override bool IsMet(NpcObj npc)
        {
            foreach (var condition in subConditions) if (!condition.IsMet(npc)) return false;

            return true;
        }
    }
}