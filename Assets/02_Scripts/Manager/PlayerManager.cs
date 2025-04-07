using UnityEngine;

public class PlayerManager
{
    public static Transform PlayerTransform { get; private set; } = null;
    public static void SetPlayerTransform(Transform playerTransform) => PlayerTransform = playerTransform;
}