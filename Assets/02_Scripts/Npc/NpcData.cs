using Dialogue;
using System.Collections.Generic;
using UnityEngine;

namespace Npc
{
    [CreateAssetMenu(fileName = "NpcData", menuName = "Npc/NpcData")]
    public class NpcData : ScriptableObject
    {
        [Header("기본 정보")]
        public NpcName npcNameType;
        public string npcName;
        [TextArea, SerializeField] string description;

        [Header("호감도 관리")]
        public NpcAffinityTracker affinityTracker;  // 호감도 증가 / 이벤트 정의

        [Header("대사 시스템")]
        public ConditionalDialogueManager dialogueManager; // 대화 조건/트리 관리

        [Header("기타 설정")]
        public Sprite npcPortrait;                  // UI에서 쓸 NPC 초상화
    }
}
