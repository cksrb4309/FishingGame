using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class Item_CustomEditor : Editor
{
    GUIStyle titleStyle;
    GUIStyle labelStyle;

    private void InitStyles()
    {
        ExtensionCustomEditor.GetStyle(ref titleStyle, CustomEditorStyle.Title);
        ExtensionCustomEditor.GetStyle(ref labelStyle, CustomEditorStyle.Label);
    }

    public override void OnInspectorGUI()
    {
        InitStyles();

        Item item = (Item)target;

        GUILayout.Space(20);
        EditorGUILayout.LabelField("아이템 데이터", titleStyle);
        GUILayout.Space(20);

        ExtensionCustomEditor.DrawCategoryField("아이템 카테고리", ref item.category, labelStyle);
        ExtensionCustomEditor.DrawIntField("아이템 아이디", ref item.itemId, labelStyle);
        ExtensionCustomEditor.DrawStringField("아이템 이름", ref item.itemName, labelStyle);
        ExtensionCustomEditor.DrawStringMultiLineField("아이템 설명", ref item.itemExplain, labelStyle);
        ExtensionCustomEditor.DrawIntField("아이템 레벨", ref item.itemLevel, labelStyle);
        ExtensionCustomEditor.DrawSpriteField("아이템 아이콘", ref item.itemIcon, labelStyle);
        ExtensionCustomEditor.DrawFishingMethodDataField("아이템 낚시 데이터", ref item.fishingMethodData, labelStyle);
        ExtensionCustomEditor.DrawBaitLootTableField("아이템 낚시 데이터", ref item.baitLootTable, labelStyle);
    }
}
