using Quest;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestGiver
{
    public void SetQuestState(QuestInfo questInfo, QuestState nextState);
}
