using UnityEngine;

public class DialogueLine
{
    public string speakerName; // 누가 말하는지
    [TextArea] public string content; // 대사 내용
    public float displayTime = 2f; // 선택: 대사 노출 시간
}
