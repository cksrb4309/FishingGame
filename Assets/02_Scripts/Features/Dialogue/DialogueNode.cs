using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode : ScriptableObject
{
    public string nodeId;

    // 초기 세팅 (대화 시작 시 연출용)
    public Sprite leftCharacterPortrait;
    public Sprite rightCharacterPortrait;
    public string leftCharacterName;
    public string rightCharacterName;

    public List<DialogueLine> lines = new List<DialogueLine>();
}
