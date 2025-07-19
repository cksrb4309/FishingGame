using System;
using System.Collections.Generic;

[Serializable]
public class DialogueNode
{
    public string text;
    public List<DialogueChoice> choices;
    public DialogueActionType actionType;

    public DialogueNode(string text)
    {
        this.text = text;
        choices = new List<DialogueChoice>();
    }
}