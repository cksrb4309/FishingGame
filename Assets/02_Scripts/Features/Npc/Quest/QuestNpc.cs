using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

namespace Quest
{
    public class QuestNpc : MonoBehaviour, IQuestNpc
    {
        [SerializeField] NpcName npcName;

        private InteractGuideImage guideImage;

        [NonSerialized] public QuestState currentState = QuestState.Inactive;

        [SerializeField] private List<QuestInfo> currentQuestStates = new();

        public void SetQuestState(QuestInfo questInfo, QuestState questState)
        {
            if (questState == QuestState.Completed)
            {
                currentQuestStates.Remove(questInfo);
            }
            else if (!currentQuestStates.Contains(questInfo))
            {
                currentQuestStates.Add(questInfo);
            }

            bool F(QuestState questState)
            {
                if (currentQuestStates.Any(
                    q => q.questState == questState &&
                    (questState == QuestState.Active ?
                    (q.questNpcInfo.giverName == npcName) : (q.questNpcInfo.receiverName == npcName))))
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


            // 만약 현재 상태가 Active, Accepted, Completable 이였다면
            if (currentState == QuestState.Active || currentState == QuestState.Accepted || currentState == QuestState.Completable)
            {
                Debug.Log("제거 : " + currentState.ToString());

                // 현재 띄우는 가이드 이미지에서 현재 상태에 대한 이미지를 제거함
                guideImage.Hide(QuestManager.Instance.questStateSprites[currentState]);
            }
        }
        private void SetState(QuestState questState)
        {
            // 이미 해당 상태라면 종료
            if (currentState == questState) return;

            // 만약 현재 상태가 Active, Accepted, Completable 이였다면
            if (currentState == QuestState.Active || currentState == QuestState.Accepted || currentState == QuestState.Completable)
            {
                Debug.Log("제거 : " + currentState.ToString());

                // 현재 띄우는 가이드 이미지에서 현재 상태에 대한 이미지를 제거함
                guideImage.Hide(QuestManager.Instance.questStateSprites[currentState]);
            }
            
            // 만약 다음 상태가 Active, Accepted, Completable일 경우엔
            if (questState == QuestState.Active || questState == QuestState.Accepted || questState == QuestState.Completable)
            {
                Debug.Log("표시 : " + questState.ToString());

                // 다음 상태에 대한 가이드 이미지를 띄운다
                guideImage.Show(QuestManager.Instance.questStateSprites[questState]);
            }
            
            // 현재 상태에 다음 상태 값 반영
            currentState = questState;
        }
        private void Start()
        {
            guideImage = GetComponent<InteractGuideImage>();
        }
    }
}
