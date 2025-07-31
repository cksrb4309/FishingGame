using System;
using UnityEditor;
using UnityEngine;

public static class ExtensionCustomEditor
{
    public static void DrawIntField(string label, ref int value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.IntField(value);
        GUILayout.EndHorizontal();
    }

    public static void DrawFloatField(string label, ref float value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.FloatField(value);
        GUILayout.EndHorizontal();
    }
    public static void DrawStringField(string label, ref string value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.TextField(value);
        GUILayout.EndHorizontal();
    }
    public static void DrawStringMultiLineField(string label, ref string value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.TextArea(value);
        GUILayout.EndHorizontal();
    }
    public static void DrawSpriteField(string label, ref Sprite value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.ObjectField(value, typeof(Sprite), false) as Sprite;
        GUILayout.EndHorizontal();
    }
    public static void DrawFishingMethodDataField(string label, ref FishingMethodData value, GUIStyle labelStyle)
    {
        if (value == null)
        {
            value = ScriptableObject.CreateInstance<FishingMethodData>();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.ObjectField(value, typeof(FishingMethodData), false) as FishingMethodData;
        GUILayout.EndHorizontal();
    }
    public static void DrawCategoryField(string label, ref ItemCategory value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = (ItemCategory)EditorGUILayout.EnumPopup(value);
        GUILayout.EndHorizontal();
    }
    public static void DrawBaitLootTableField(string label, ref BaitLootTable value, GUIStyle labelStyle)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(150));
        EditorGUIUtility.labelWidth = 0;
        value = EditorGUILayout.ObjectField(value, typeof(BaitLootTable), false) as BaitLootTable;
        GUILayout.EndHorizontal();
    }
    private static void SetTitleStyle(ref GUIStyle style)
    {
        style = new GUIStyle(EditorStyles.label)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };
    }
    private static void SetLabelStyle(ref GUIStyle style)
    {
        style = new GUIStyle(EditorStyles.label)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(0.9f, 0.9f, 0.9f) }
        };
    }
    public static void GetStyle(ref GUIStyle style, CustomEditorStyle type)
    {
        switch (type){
            case CustomEditorStyle.Title: SetTitleStyle(ref style); break;
            case CustomEditorStyle.Label: SetLabelStyle(ref style); break;
        }
    }
    public static void DrawEnumPopup<T>(string label, ref T enumValue, GUIStyle labelStyle) where T : Enum
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, labelStyle, GUILayout.Width(150));
        enumValue = (T)EditorGUILayout.EnumPopup(enumValue);
        EditorGUILayout.EndHorizontal();
    }
}
public enum CustomEditorStyle
{
    Title,
    Label,
}
