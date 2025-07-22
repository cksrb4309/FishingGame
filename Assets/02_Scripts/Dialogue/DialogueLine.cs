using Dialogue;
using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class DialogueLine
    {
        [Multiline] public string text;
        public DialoguePosition position;    // 왼쪽 or 오른쪽
        public NpcName speakerNpcId;             // 발화자 ID (NpcData 조회용)
    }
}
