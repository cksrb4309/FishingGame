﻿#if UNITY_EDITOR
using UnityEditor;
using System;

namespace GSpawn_Pro
{
    [Serializable]
    public class ObjectRotationGizmo : ObjectTransformGizmo
    {
        protected override Channels draw()
        {
            EditorGUI.BeginChangeCheck();
            _newRotation = Handles.RotationHandle(rotation, position);
            if (EditorGUI.EndChangeCheck()) return Channels.Rotation;

            return Channels.None;
        }
    }
}
#endif