using UnityEngine;

[System.Serializable]
public class PreviousQuestCompletedCondition : QuestCondition
{
    [SerializeField] QuestInfo previousQuest;
    public override bool IsMet()
    {
        return previousQuest.isComplete;
    }
}