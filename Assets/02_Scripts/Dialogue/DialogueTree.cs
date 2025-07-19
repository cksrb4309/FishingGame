using UnityEngine;

[CreateAssetMenu(fileName = "DialogueTree", menuName = "Dialogue/DialogueTree")]
public class DialogueTree : ScriptableObject
{
    public DialogueNode startNode;
}