using UnityEngine;

[System.Serializable]
public class AffinityCondition : QuestCondition
{
    [SerializeField] int npcId;
    [SerializeField] int requiredAffinity;
    public override bool IsMet()
    {
        return requiredAffinity <= NpcAffinitySystem.GetAffinity(npcId);
    }
}