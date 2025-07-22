using System.Collections.Generic;
using UnityEngine;

namespace Quest
{
    public class QuestUnlockConditionManager
    {
        private static QuestUnlockConditionManager _instance;
        public static QuestUnlockConditionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QuestUnlockConditionManager();

                return _instance;
            }
        }

        private Dictionary<Item, List<QuestInfo>> itemUnlockQuests = new Dictionary<Item, List<QuestInfo>>();
        private Dictionary<NpcName, List<QuestInfo>> npcAffinityUnlockQuests = new Dictionary<NpcName, List<QuestInfo>>();
        private Dictionary<int, List<QuestInfo>> questCompleteUnlockQuests = new Dictionary<int, List<QuestInfo>>();

        private QuestUnlockConditionManager() { } // 寇何俊辑 积己 规瘤

        public void OnItemObtained(Item item)
        {
            if (itemUnlockQuests.TryGetValue(item, out var quests))
            {
                foreach (var quest in quests)
                    TryUnlockQuest(quest);

                itemUnlockQuests.Remove(item);
            }
        }

        public void OnNpcAffinityChanged(NpcName npcName, int currentAffinity)
        {
            if (npcAffinityUnlockQuests.TryGetValue(npcName, out var quests))
            {
                foreach (var quest in quests)
                    TryUnlockQuest(quest);

                npcAffinityUnlockQuests.Remove(npcName);
            }
        }

        public void OnQuestCompleted(int questId)
        {
            if (questCompleteUnlockQuests.TryGetValue(questId, out var quests))
            {
                foreach (var quest in quests)
                    TryUnlockQuest(quest);

                questCompleteUnlockQuests.Remove(questId);
            }
        }

        private void TryUnlockQuest(QuestInfo quest)
        {
            QuestManager.Instance.ActivateQuest(quest);
        }

        public void RegisterItemCondition(Item item, QuestInfo quest)
        {
            if (!itemUnlockQuests.ContainsKey(item))
                itemUnlockQuests[item] = new List<QuestInfo>();

            itemUnlockQuests[item].Add(quest);
        }

        public void RegisterAffinityCondition(NpcName npcName, QuestInfo quest)
        {
            if (!npcAffinityUnlockQuests.ContainsKey(npcName))
                npcAffinityUnlockQuests[npcName] = new List<QuestInfo>();

            npcAffinityUnlockQuests[npcName].Add(quest);
        }

        public void RegisterQuestCompleteCondition(int questId, QuestInfo quest)
        {
            if (!questCompleteUnlockQuests.ContainsKey(questId))
                questCompleteUnlockQuests[questId] = new List<QuestInfo>();

            questCompleteUnlockQuests[questId].Add(quest);
        }
    }
}