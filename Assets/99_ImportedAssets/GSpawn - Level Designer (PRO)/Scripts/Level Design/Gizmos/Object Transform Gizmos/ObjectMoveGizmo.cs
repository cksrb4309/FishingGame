﻿#if UNITY_EDITOR
using UnityEditor;
using System;

namespace GSpawn_Pro
{
    [Serializable]
    public class ObjectMoveGizmo : ObjectTransformGizmo
    {
        protected override Channels draw()
        {
            EditorGUI.BeginChangeCheck();
            _newPosition = Handles.PositionHandle(position, rotation);
            if (EditorGUI.EndChangeCheck()) return Channels.Position;

            return Channels.None;
        }
    }
}
#endif