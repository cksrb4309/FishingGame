using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Quest
{
    public class QuestGiver : MonoBehaviour, IQuestGiver
    {
        public QuestInfo questInfo;

        [Header("UI 아이콘")]
        [SerializeField] private SpriteRenderer questAvailableIcon;
        [SerializeField] private SpriteRenderer questInProgressIcon;
        [SerializeField] private SpriteRenderer questCompletableIcon;

        [NonSerialized] public QuestState currentState = QuestState.Inactive;

        public void SetQuest(QuestInfo quest)
        {
            questInfo = quest;
        }

        public void SetQuestState(QuestState nextState)
        {
            Debug.Log($"퀘스트 상태 갱신: {nextState}");
            currentState = nextState;

            switch (nextState)
            {
                case QuestState.Inactive: SetIcon(false, false, false); break;
                case QuestState.Active: SetIcon(true, false, false); break;
                case QuestState.Accepted: SetIcon(false, true, false); break;
                case QuestState.Completable: SetIcon(false, false, true); break;
                case QuestState.Completed: SetIcon(false, false, false); break;
            }
        }
        public void Accept()
        {
            QuestManager.Instance.AcceptQuest(questInfo);
        }

        public void Decline()
        {
            Debug.Log($"퀘스트 거절: {questInfo.questName}");
        }

        private void SetIcon(bool available, bool inProgress, bool completable)
        {
            questAvailableIcon.gameObject.SetActive(available);
            questInProgressIcon.gameObject.SetActive(inProgress);
            questCompletableIcon.gameObject.SetActive(completable);
        }
    }
}

