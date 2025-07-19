using System;
using UnityEngine;

[Serializable]
public class ConditionalDialogueTree
{
    public DialogueConditionType conditionType;   // 어떤 조건을 체크할지
    public DialogueTree dialogueTree;             // 조건 만족 시 사용할 대화 트리

    public bool CheckCondition(Npc npc)
    {
        switch (conditionType)
        {
            case DialogueConditionType.HasGivenGift: return npc.HasGivenGift;

            case DialogueConditionType.QuestCompleted: return QuestManager.Instance.IsQuestCompleted(npc.LinkedQuestID);

            case DialogueConditionType.AffinityAboveThreshold: return npc.Affinity >= requiredAffinity;

            default: return false;
        }
    }

    [SerializeField] private int requiredAffinity;   // 필요 호감도 (옵션)
}
