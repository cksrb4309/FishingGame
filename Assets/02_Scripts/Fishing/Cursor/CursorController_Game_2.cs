using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController_Game_2 : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform offsetPosition;

    InputActionReference mousePositionInput = null;

    Vector3 initLocalPosition;
    private void Awake()
    {
        initLocalPosition = offsetPosition.localPosition;
    }
    public void Setting()
    {
        gameObject.SetActive(true);

        offsetPosition.localPosition = initLocalPosition + Vector3.down * FishingData.MiniGame_2_Data.AttackRange;

        enabled = true;
    }
    private void LateUpdate()
    {
        Vector2 mouseScreenPos = mousePositionInput.action.ReadValue<Vector2>();
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        targetWorldPos.z = offsetPosition.position.z; // 2D UI 기준 z 제거

        targetPosition.position = offsetPosition.position + (targetWorldPos - offsetPosition.position).normalized * 10f;

        Vector3 dir = targetWorldPos - offsetPosition.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f; // y축이 위쪽이므로, x축 기준 회전에서 90도 보정

        offsetPosition.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void Cancel()
    {
        offsetPosition.gameObject.SetActive(false);
        enabled = false;
    }
    private void OnEnable()
    {
        mousePositionInput = InputManager.GetInputAction(InputType.MousePoint);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.MousePoint);
    }
}
