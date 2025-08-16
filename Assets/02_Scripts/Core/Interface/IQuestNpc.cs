using Quest;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestNpc
{
    public void SetQuestState(QuestInfo questInfo, QuestState nextState);
}
