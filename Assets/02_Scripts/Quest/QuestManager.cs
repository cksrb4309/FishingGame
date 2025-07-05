using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; } = null;

    public QuestDatabase questDatabase;

    public SerializedDictionary<int, GameObject> questGivers;

    List<QuestInfo> inactiveQuests = new();
    List<QuestInfo> availableQuests = new();
    List<QuestInfo> ongoingQuests = new();
    List<QuestInfo> completedQuests = new();

    Dictionary<Item, List<QuestInfo>> itemToQuests = new();

    private void Start()
    {
        Instance = this;

        Init();
    }
    void Init()
    {
        foreach (QuestInfo questInfo in questDatabase)
        {
            if (questInfo.IsAcceptable())
            {
                Debug.Log("퀘스트 활성화");
                ActivateQuest(questInfo);
            }

            else
            {
                Debug.Log("퀘스트 비활성화");
                DeactivateQuest(questInfo);
            }
        }
    }
    void ActivateQuest(QuestInfo questInfo)
    {
        // 비활성화 퀘스트에서 제거
        inactiveQuests.RemoveAll(q => q.questId == questInfo.questId);

        availableQuests.Add(questInfo);

        SetQuestGiverState(questInfo, QuestState.Active);
    }
    void DeactivateQuest(QuestInfo questInfo)
    {
        inactiveQuests.Add(questInfo);

        SetQuestGiverState(questInfo, QuestState.Inactive);
    }
    public void AcceptQuest(QuestInfo questInfo)
    {
        // 활성화 퀘스트에서 제거
        availableQuests.RemoveAll(q => q.questId == questInfo.questId);

        AddTargetQuest(questInfo);

        // 수행 중 퀘스트에 추가
        ongoingQuests.Add(questInfo);

        SetQuestGiverState(questInfo, questInfo.IsCompletable() ? QuestState.Completable : QuestState.Accepted);
    }
    public void CompleteQuest(QuestInfo questInfo)
    {
        // 수행 중 퀘스트에서 제거
        ongoingQuests.RemoveAll(q => q.questId == questInfo.questId);

        RemoveTargetQuest(questInfo);

        // 완료 퀘스트에 추가
        completedQuests.Add(questInfo);

        PlayerInventory.Instance.UseQuestItem(questInfo);

        // 퀘스트 보상 처리
        PlayerInventory.Instance.GetQuestReward(questInfo);

        SetQuestGiverState(questInfo, QuestState.Completed);
    }
    void AddTargetQuest(QuestInfo questInfo)
    {
        // 퀘스트 아이템 리스트 가져오기
        List<QuestItem> questItems = questInfo.questItems;

        for (int i = 0; i < questItems.Count; i++)
        {
            if (!itemToQuests.ContainsKey(questItems[i].targetItem))
                itemToQuests[questItems[i].targetItem] = new();

            // 아이템에 대한 대상 퀘스트로 추가한다
            itemToQuests[questItems[i].targetItem].Add(questInfo);
        }
    }
    void RemoveTargetQuest(QuestInfo questInfo)
    {
        // 퀘스트 아이템 리스트 가져오기
        List<QuestItem> questItems = questInfo.questItems;

        for (int i = 0; i < questItems.Count; i++)
        
            itemToQuests[questItems[i].targetItem].RemoveAll(q => q.questId == questInfo.questId);
    }
    void SetQuestGiverState(QuestInfo questInfo, QuestState questState)
    {
        questGivers[questInfo.questId].GetComponent<IQuestGiver>().SetQuestState(questState);
    }
    public void CheckCompletableQuest(Item item)
    {
        if (itemToQuests.ContainsKey(item))
            for (int i = 0; i < itemToQuests[item].Count; i++)
                if (itemToQuests[item][i].IsCompletable())
                    SetQuestGiverState(itemToQuests[item][i], QuestState.Completable);
    }
}
