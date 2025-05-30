using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerFishingRodController))]
public class PlayerFishingControllerEditor : Editor
{
    bool isEditing = false;
    PlayerDir selectedDir = PlayerDir.Top; // 수정하고 싶은 방향 선택 변수

    void OnSceneGUI()
    {
        PlayerFishingRodController controller = (PlayerFishingRodController)target;

        if (!isEditing || !controller.directionSettings.ContainsKey(selectedDir))
            return;

        var dirSet = controller.directionSettings[selectedDir];

        Vector3 worldPivot = controller.transform.position + dirSet.pivotPosition;
        Vector3 worldTip = controller.transform.position + dirSet.tipPosition;
        Vector3 worldTension = controller.transform.position + dirSet.defaultTensionPosition;

        EditorGUI.BeginChangeCheck();

        Vector3 newPivot = Handles.PositionHandle(worldPivot, Quaternion.identity);
        Vector3 newTip = Handles.PositionHandle(worldTip, Quaternion.identity);
        Vector3 newTension = Handles.PositionHandle(worldTension, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(controller, "Move Fishing Rod Points");

            dirSet.pivotPosition = newPivot - controller.transform.position;
            dirSet.tipPosition = newTip - controller.transform.position;
            dirSet.defaultTensionPosition = newTension - controller.transform.position;

            controller.directionSettings[selectedDir] = dirSet;

            EditorUtility.SetDirty(controller);
        }

        Handles.color = Color.green;
        Handles.DrawLine(newPivot, newTip);
        Handles.DrawLine(controller.transform.position, newTension);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        EditorGUILayout.LabelField("🎯 편집할 방향 선택", EditorStyles.boldLabel);
        selectedDir = (PlayerDir)EditorGUILayout.EnumPopup("편집 방향", selectedDir);

        GUILayout.Space(5);
        isEditing = GUILayout.Toggle(isEditing, isEditing ? "🛑 편집 중지" : "🎣 편집 시작", "Button");

        if (GUI.changed)
        {
            SceneView.RepaintAll();
        }
    }
}