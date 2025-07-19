using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "Quest/QuestInfo")]
public class QuestInfo : ScriptableObject
{
    public int questId; // 퀘스트 아이디

    public List<QuestItem> questItems; // 퀘스트 아이템
    public List<QuestItem> rewardItems; // 보상 아이템

    public string questName;

    [Multiline] public string questDescription;

    public QuestCondition acceptCondition;

    [NonSerialized] public bool isComplete = false;

    public bool IsAcceptable()
    {
        return acceptCondition == null || acceptCondition.IsMet();
    }
    public bool IsCompletable()
    {
        bool isComplete = true;

        for (int i = 0; i < questItems.Count; i++) isComplete = isComplete && questItems[i].HasEnoughItems();

        return isComplete;
    }
}
