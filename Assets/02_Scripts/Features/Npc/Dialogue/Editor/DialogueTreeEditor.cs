using Dialogue;   // 네가 네임스페이스로 감쌌다면 필요
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueTree))]
public class DialogueTreeEditor : Editor
{
    //private DialogueTree tree;
    //private HashSet<DialogueNode> visitedNodes = new HashSet<DialogueNode>();
    //private void OnEnable()
    //{
    //    tree = (DialogueTree)target;
    //}

    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    EditorGUILayout.Space(20);
    //    EditorGUILayout.LabelField("Dialogue Tree View", EditorStyles.boldLabel);

    //    if (tree.startNode == null)
    //    {
    //        EditorGUILayout.HelpBox("Start Node가 비어 있습니다.", MessageType.Warning);
    //        return;
    //    }

    //    visitedNodes.Clear();
    //    DrawNodeRecursive(tree.startNode, 0);
    //}

    //private void DrawNodeRecursive(DialogueNode node, int indentLevel)
    //{
    //    if (node == null || visitedNodes.Contains(node))
    //        return;

    //    visitedNodes.Add(node);

    //    EditorGUI.indentLevel = indentLevel;

    //    EditorGUILayout.BeginVertical("box");
    //    EditorGUILayout.LabelField($"Node: {node.text.Substring(0, Mathf.Min(30, node.text.Length))}...", EditorStyles.boldLabel);

    //    EditorGUILayout.LabelField($"Speaker ID: {node.speakerNpcId} ({node.position})");
    //    EditorGUILayout.LabelField($"Choices: {node.choices?.Count ?? 0}");

    //    EditorGUILayout.EndVertical();

    //    if (node.choices != null)
    //    {
    //        foreach (var choice in node.choices)
    //        {
    //            EditorGUILayout.Space(5);
    //            EditorGUI.indentLevel = indentLevel + 1;
    //            EditorGUILayout.LabelField($"▶ Choice: {choice.choiceText}");

    //            DrawNodeRecursive(choice.nextNode, indentLevel + 2);
    //        }
    //    }
    //}
}