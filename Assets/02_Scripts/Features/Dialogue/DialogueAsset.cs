using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueAsset")]
public class DialogueAsset : ScriptableObject
{
    public List<DialogueNode> nodes;
}
