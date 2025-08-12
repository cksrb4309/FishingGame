using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "AffinityCondition", menuName = "Quest/Condition/Affinity")]
    public class AffinityCondition : QuestCondition
    {
        public NpcName npcName;
        public int requiredAffinity;
        public override bool IsMet()
        {
            return requiredAffinity <= Npc.NpcAffinitySystem.GetAffinity(npcName);
        }
        public override void Register(QuestInfo questInfo)
        {
            QuestUnlockConditionManager.Instance.RegisterAffinityCondition(npcName, questInfo);
        }
    }
}
