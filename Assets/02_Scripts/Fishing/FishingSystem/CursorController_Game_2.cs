using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController_Game_2 : MonoBehaviour
{
    [SerializeField] Transform offsetPosition;
    [SerializeField] Image cursorImage;

    InputActionReference mousePositionInput = null;

    public void Setting()
    {
        cursorImage.gameObject.SetActive(true);
        enabled = true;
    }
    private void LateUpdate()
    {
        Vector2 mouseScreenPos = mousePositionInput.action.ReadValue<Vector2>();
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        targetWorldPos.z = cursorImage.transform.position.z; // 2D UI 기준 z 제거

        offsetPosition.position = cursorImage.transform.position + (targetWorldPos - cursorImage.transform.position).normalized * 10f;

        Vector3 dir = targetWorldPos - cursorImage.transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f; // y축이 위쪽이므로, x축 기준 회전에서 90도 보정

        cursorImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void Cancel()
    {
        cursorImage.gameObject.SetActive(false);
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
