using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

namespace Quest
{
    public class QuestGiver : MonoBehaviour, IQuestGiver
    {
        private InteractGuideImage guideImage;
        [SerializeField] private SerializedDictionary<QuestState, Sprite> questStateSprites;

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
            if (currentState == questState) return;

            if (currentState == QuestState.Active || currentState == QuestState.Accepted || currentState == QuestState.Completable)
            
                guideImage.Hide(questStateSprites[currentState]);
            
            if (questState == QuestState.Active || questState == QuestState.Accepted || questState == QuestState.Completable)
            
                guideImage.Show(questStateSprites[questState]);
            
            currentState = questState;
        }
        private void Start()
        {
            guideImage = GetComponent<InteractGuideImage>();
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
