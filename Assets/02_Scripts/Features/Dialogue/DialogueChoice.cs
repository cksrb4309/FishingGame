using System;

[Serializable]
public class DialogueChoice
{
    public string choiceText;       // UI에 표시되는 선택지 텍스트
    public DialogueNode nextNode;   // 선택 시 이동할 다음 노드
    public string impactFlag;       // 엔딩에 영향을 주는 플래그(예: "HelpedNPC")
}
