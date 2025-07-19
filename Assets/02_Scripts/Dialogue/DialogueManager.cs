using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private DialogueNode currentNode;

    public void StartDialogue(DialogueTree dialogueTree)
    {
        DisplayNode(dialogueTree.startNode);
    }

    private void DisplayNode(DialogueNode node)
    {
        currentNode = node;

        for (int i = 0; i < node.choices.Count; i++)
        {
            Debug.Log($"[{i}] {node.choices[i].choiceText}");
        }
    }

    public void SelectChoice(int index)
    {
        if (currentNode == null || index < 0 || index >= currentNode.choices.Count)
            return;

        DialogueChoice choice = currentNode.choices[index];

        choice.onChoiceSelected?.Invoke();

        DisplayNode(choice.nextNode);
    }
}