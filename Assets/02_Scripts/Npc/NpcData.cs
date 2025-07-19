using UnityEngine;

[CreateAssetMenu(fileName = "NpcData", menuName = "Npc/NpcData")]
public class NpcData : ScriptableObject
{
    public int npcId;
    public string npcName;

    public NpcAffinityTracker affinityTracker; // 호감도 이벤트 데이터
}