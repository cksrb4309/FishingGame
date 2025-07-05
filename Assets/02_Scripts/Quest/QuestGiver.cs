using System;
using Unity.VisualScripting;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IQuestGiver
{
    public QuestInfo questInfo;

    [NonSerialized] public QuestState currentState = QuestState.Inactive;

    [SerializeField] SpriteRenderer questAvailableIcon;
    [SerializeField] SpriteRenderer questInProgressIcon;
    [SerializeField] SpriteRenderer questCompletableIcon;

    public void SetQuestState(QuestState nextState)
    {
        Debug.Log("퀘스트 상태 갱신 : " + nextState.ToString());

        switch (nextState)
        {
            case QuestState.Inactive: SetIcon(false, false, false); break;

            case QuestState.Active: SetIcon(true, false, false); break;

            case QuestState.Accepted: SetIcon(false, true, false); break;

            case QuestState.Completable: SetIcon(false, false, true); break;

            case QuestState.Completed: SetIcon(false, false, false); break;
        }
        currentState = nextState;
    }
    public void Interact()
    {
        Debug.Log("상호작용 확인 ");
        switch (currentState)
        {
            case QuestState.Inactive: break;

            case QuestState.Active: QuestUIController.Instance.ShowQuestUI(this); break;

            case QuestState.Accepted: break;

            case QuestState.Completable: QuestManager.Instance.CompleteQuest(questInfo); break;

            case QuestState.Completed: break;
        }
    }
    public void Accept()
    {
        QuestManager.Instance.AcceptQuest(questInfo);
    }
    public void Decline()
    {
        Debug.Log("퀘스트 거절 : " + questInfo.questName);
    }
    void SetIcon(bool availableIconActive, bool inProgressIconActive, bool completableIconActive)
    {
        Debug.Log("아이콘 셋팅 : " + availableIconActive.ToString() + " , " + inProgressIconActive.ToString() + " , " + completableIconActive.ToString());
        questAvailableIcon.gameObject.SetActive(availableIconActive);
        questInProgressIcon.gameObject.SetActive(inProgressIconActive);
        questCompletableIcon.gameObject.SetActive(completableIconActive);
    }
}
