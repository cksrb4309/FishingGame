using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    public static DirectionManager Instance { get; private set; } = null;

    [SerializeField] List<IDirectionable> directionables = new();

    [SerializeField] Transform pivotTransform;

    InputActionReference mousePointInputActionReference;

    PlayerDir currentDir = PlayerDir.TopRight;

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

        if ((angle >= 0f && angle < 90f) && currentDir != PlayerDir.TopRight) { currentDir = PlayerDir.TopRight; SetAllDirections(); }
        else if ((angle >= 90f && angle < 180f) && currentDir != PlayerDir.TopLeft) { currentDir = PlayerDir.TopLeft; SetAllDirections(); }
        else if ((angle >= 180f && angle < 270f) && currentDir != PlayerDir.BottomLeft) { currentDir = PlayerDir.BottomLeft; SetAllDirections(); }
        else if ((angle >= 270f && angle < 360f) && currentDir != PlayerDir.BottomRight) { currentDir = PlayerDir.BottomRight; SetAllDirections(); }
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


