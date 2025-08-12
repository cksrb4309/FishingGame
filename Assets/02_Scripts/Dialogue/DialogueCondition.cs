using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public abstract class DialogueCondition : ScriptableObject
    {
        public abstract bool IsMet(Npc.NpcObj npc);
    }
}
