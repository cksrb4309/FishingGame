using Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; } = null;

        public QuestDatabase questDatabase;

        public SerializedDictionary<NpcName, GameObject> npcDictionary;

        public SerializedDictionary<QuestState, Sprite> questStateSprites;

        List<QuestInfo> availableQuests = new();
        List<QuestInfo> ongoingQuests = new();
        List<QuestInfo> completedQuests = new();

        Dictionary<Item, List<QuestInfo>> itemToQuests = new();

        public static QuestInfo completeQuest = null;
        public static int questPriority = -1;
        private void Start()
        {
            Instance = this;

            Init();
        }
        void Init()
        {
            foreach (QuestInfo questInfo in questDatabase)
            {
                questInfo.Register();
            }
        }
        public void ActivateQuest(QuestInfo questInfo)
        {
            availableQuests.Add(questInfo);

            SetQuestState(questInfo, QuestState.Active);
        }
        public void DeactivateQuest(QuestInfo questInfo)
        {
            SetQuestState(questInfo, QuestState.Inactive);
        }
        public void AcceptQuest(QuestInfo questInfo)
        {
            // 활성화 퀘스트에서 제거
            availableQuests.RemoveAll(q => q.questId == questInfo.questId);

            AddTargetQuest(questInfo);

            // 수행 중 퀘스트에 추가
            ongoingQuests.Add(questInfo);

            SetQuestState(questInfo, questInfo.IsCompletable() ? QuestState.Completable : QuestState.Accepted);
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

            SetQuestState(questInfo, QuestState.Completed);
        }
        public void CompleteQuest()
        {
            questPriority = -1;

            CompleteQuest(completeQuest);
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
        void SetQuestState(QuestInfo questInfo, QuestState questState)
        {
            questInfo.questState = questState;

            npcDictionary[questInfo.questNpcInfo.giverName].GetComponent<IQuestNpc>().SetQuestState(questInfo, questState);
            npcDictionary[questInfo.questNpcInfo.receiverName].GetComponent<IQuestNpc>().SetQuestState(questInfo, questState);
        }
        public void CheckCompletableQuest(Item item)
        {
            if (itemToQuests.ContainsKey(item))
                for (int i = 0; i < itemToQuests[item].Count; i++)
                    if (itemToQuests[item][i].IsCompletable())
                        SetQuestState(itemToQuests[item][i], QuestState.Completable);
        }
        public bool IsQuestCompleted(int questId)
        {
            return completedQuests.Any(quest => quest.questId == questId);
        }
        public bool IsQuestInProgress(int questId)
        {
            return ongoingQuests.Any(quest => quest.questId == questId);
        }
        public bool HasAvailableQuestFromNpc(NpcName npcName, out DialogueTree questDialogueTree)
        {
            foreach (var quest in availableQuests)
            {
                if (quest.questNpcInfo.giverName == npcName && quest.questState == QuestState.Active)
                {
                    questDialogueTree = quest.startQuestDialogueTree;

                    QuestUIController.quest = quest;

                    return true;
                }
            }

            questDialogueTree = null;
            return false;
        }
    }

    [Serializable]
    public class QuestNpcInfo
    {
        public NpcName giverName;
        public NpcName receiverName;

        //public GameObject giverObj;
        //public GameObject receiverObj;

        //public void SetQuestNpcState(QuestInfo questInfo, QuestState questState)
        //{
        //    giverObj.GetComponent<IQuestNpc>().SetQuestState(questInfo, questState, true);
        //    receiverObj.GetComponent<IQuestNpc>().SetQuestState(questInfo, questState, false);
        //}
    }
}
