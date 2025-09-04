using UnityEngine;

public class DialogueLine
{
    public PortraitPosition portraitPosition;
    public NpcName speakerName;
    public PortraitType portraitType;
    [TextArea] public string content; // 대사 내용
}
