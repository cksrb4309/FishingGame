using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    public static DirectionManager Instance { get; private set; } = null;

    [SerializeField] List<IDirectionable> directionables = new();

    [SerializeField] Transform pivotTransform;

    InputActionReference mousePointInputActionReference;

    PlayerDir currentDir = PlayerDir.Bottom;

    bool isDirectionUpdate = true;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        pivotTransform = transform;
    }
    public void Update()
    {
        if (!isDirectionUpdate) return;

        Vector2 playerPosition = Camera.main.WorldToScreenPoint(pivotTransform.position);
        float angle = LookAtMouseUtils2D.GetLookAtMouseAngle(playerPosition);
        if (angle < 0) angle += 360f;

        if (angle >= 337.5f || angle < 22.5f) { if (currentDir != PlayerDir.Right) { currentDir = PlayerDir.Right; SetAllDirections(); } }
        else if (angle >= 22.5f && angle < 67.5f) { if (currentDir != PlayerDir.TopRight) { currentDir = PlayerDir.TopRight; SetAllDirections(); } }
        else if (angle >= 67.5f && angle < 112.5f) { if (currentDir != PlayerDir.Top) { currentDir = PlayerDir.Top; SetAllDirections(); } }
        else if (angle >= 112.5f && angle < 157.5f) { if (currentDir != PlayerDir.TopLeft) { currentDir = PlayerDir.TopLeft; SetAllDirections(); } }
        else if (angle >= 157.5f && angle < 202.5f) { if (currentDir != PlayerDir.Left) { currentDir = PlayerDir.Left; SetAllDirections(); } }
        else if (angle >= 202.5f && angle < 247.5f) { if (currentDir != PlayerDir.BottomLeft) { currentDir = PlayerDir.BottomLeft; SetAllDirections(); } }
        else if (angle >= 247.5f && angle < 292.5f) { if (currentDir != PlayerDir.Bottom) { currentDir = PlayerDir.Bottom; SetAllDirections(); } }
        else if (angle >= 292.5f && angle < 337.5f) { if (currentDir != PlayerDir.BottomRight) { currentDir = PlayerDir.BottomRight; SetAllDirections(); } }
    }
    void SetAllDirections()
    {
        for (int i = 0; i < directionables.Count; i++) directionables[i].UpdateDirection(currentDir);
    }
    public void RegisterDirectionable(IDirectionable directionable)
    {
        directionables.Add(directionable);

        directionable.UpdateDirection(currentDir);
    }
    public void UnregisterDirectionable(IDirectionable directionable)
    {
        directionables.Remove(directionable);
    }
    public void EnableDirection() => isDirectionUpdate = true;
    public void DisableDirection() => isDirectionUpdate = false;
    private void OnEnable()
    {
        mousePointInputActionReference = InputManager.GetInputAction(InputType.MousePoint);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.MousePoint);
    }
}


