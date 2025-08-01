using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Quest
{
    public class QuestGiver : MonoBehaviour, IQuestGiver
    {
        [Header("UI 아이콘")]
        [SerializeField] private SpriteRenderer questAvailableIcon;
        [SerializeField] private SpriteRenderer questInProgressIcon;
        [SerializeField] private SpriteRenderer questCompletableIcon;

        [NonSerialized] public QuestState currentState = QuestState.Inactive;

        [SerializeField] private List<QuestStatePair> currentQuestStates = new();

        public void SetQuestState(QuestInfo questInfo, QuestState questState)
        {
            QuestStatePair selectQuestState = currentQuestStates.Find(statePair => statePair.questInfo == questInfo);

            // 해당 NPC에게 퀘스트 데이터가 이미 있는 경우
            if (selectQuestState != null)
                selectQuestState.questState = questState;

            else
                currentQuestStates.Add(new QuestStatePair(questInfo, questState));

            bool F(QuestState questState)
            {
                if (currentQuestStates.Any(statePair => statePair.questState == questState))
                {
                    SetState(questState);

                    return true;
                }
                return false;
            }

            if (F(QuestState.Completable)) return;
            if (F(QuestState.Accepted)) return;
            if (F(QuestState.Active)) return;
            if (F(QuestState.Inactive)) return;
        }
        private void SetState(QuestState questState)
        {
            switch (questState)
            {
                case QuestState.Inactive: SetIcon(false, false, false); break;
                case QuestState.Active: SetIcon(true, false, false); break;
                case QuestState.Accepted: SetIcon(false, true, false); break;
                case QuestState.Completable: SetIcon(false, false, true); break;
                case QuestState.Completed: SetIcon(false, false, false); break;
            }
        }
        private void SetIcon(bool available, bool inProgress, bool completable)
        {
            questAvailableIcon.gameObject.SetActive(available);
            questInProgressIcon.gameObject.SetActive(inProgress);
            questCompletableIcon.gameObject.SetActive(completable);
        }
    }
}

public class QuestStatePair
{
    public QuestInfo questInfo;
    public QuestState questState;

    public QuestStatePair(QuestInfo questInfo, QuestState questState)
    {
        this.questInfo = questInfo;
        this.questState = questState;
    }
}
