using UnityEngine;

public enum QuestState
{
    Inactive,       // 비활성화되어 아직 퀘스트를 확인할 수 없음
    Active,         // 퀘스트를 수락할 수 있음
    Accepted,       // 퀘스트를 수락했지만 아직 완료 조건을 충족하지 않음
    Completable,    // 퀘스트 완료 조건을 충족하여 완료할 수 있음
    Completed       // 퀘스트를 완료한 상태
}
