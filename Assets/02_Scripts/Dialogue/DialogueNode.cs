using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialougeNode")]
    public class DialogueNode : ScriptableObject
    {
        public List<DialogueLine> lines;           // 대화 라인 묶음
        public List<DialogueChoice> choices;       // 이 묶음 발화가 끝난 후 등장할 선택지들
        public DialogueActionType actionType;      // (선택) 이 묶음이 끝났을 때 수행할 액션
    }
}
