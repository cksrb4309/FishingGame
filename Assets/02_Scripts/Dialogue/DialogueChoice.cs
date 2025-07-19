using System;

[Serializable]
public class DialogueChoice
{
    public string choiceText;
    public DialogueNode nextNode;
    public Action onChoiceSelected;

    public DialogueChoice(string choiceText, DialogueNode nextNode = null)
    {
        this.choiceText = choiceText;
        this.nextNode = nextNode;
    }
}