using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "DialogueCondition/AffinityCondition")]
    public class AffinityCondition : DialogueCondition
    {
        public int requiredAffinity;

        public override bool IsMet(Npc.NpcObj npc)
        {
            return npc.GetAffinity() >= requiredAffinity;
        }
    }
}

