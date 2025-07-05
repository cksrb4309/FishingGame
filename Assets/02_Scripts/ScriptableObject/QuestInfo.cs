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

    public bool IsAcceptable()
    {
        return true;
    }
    public bool IsCompletable()
    {
        bool isComplete = true;

        for (int i = 0; i < questItems.Count; i++) isComplete = isComplete && questItems[i].HasEnoughItems();

        Debug.Log("확인 : " + isComplete.ToString());

        return isComplete;
    }
}
