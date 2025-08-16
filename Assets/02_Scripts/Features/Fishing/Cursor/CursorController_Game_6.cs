using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController_Game_6 : CursorController
{
    [SerializeField] float distance = 2f;
    [SerializeField] Transform parentTransform;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform offsetPosition;

    private void LateUpdate()
    {
        Vector2 mouseScreenPos = mousePositionInput.action.ReadValue<Vector2>();
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // 2D이므로 z 고정
        targetWorldPos.z = offsetPosition.position.z;

        // 목표 위치 계산
        targetPosition.position = offsetPosition.position +
            (targetWorldPos - offsetPosition.position).normalized * distance;

        // ---- 여기서 회전 처리 ----
        Vector3 dir = targetPosition.position - offsetPosition.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, 0, 179.9f);

        offsetPosition.rotation = Quaternion.Euler(0, 0, angle);
    }
    public override void Cancel()
    {
        offsetPosition.gameObject.SetActive(false);
        enabled = false;
    }

    protected override void Shoot()
    {
        ThrushSlash thrushSlash = PoolManager.GetObj<ThrushSlash>(ObjectPoolID.ThrushSlash);

        if (!thrushSlash.gameObject.activeSelf) thrushSlash.gameObject.SetActive(true);

        thrushSlash.transform.SetParent(parentTransform);

        thrushSlash.transform.position = targetPosition.position;
        thrushSlash.transform.rotation = offsetPosition.rotation;

        thrushSlash.transform.rotation = Quaternion.Euler(0, 0, thrushSlash.transform.rotation.eulerAngles.z - 90f);

        thrushSlash.Setting();
    }

    protected override void SetAttackCooltime()
    {
        isShoot = true;
    }
}
