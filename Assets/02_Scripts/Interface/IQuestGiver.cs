using System.Collections.Generic;
using UnityEngine;

public interface IQuestGiver
{
    public void SetQuestState(QuestState questState);
    public void Interact();
}
