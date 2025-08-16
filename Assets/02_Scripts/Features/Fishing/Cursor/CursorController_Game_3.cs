using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController_Game_3 : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform offsetPosition;

    InputActionReference mousePositionInput = null;
    InputActionReference attackInput = null;

    public void Setting()
    {
        gameObject.SetActive(true);
        enabled = true;
    }
    private void Update()
    {
        if (attackInput.action.WasPressedThisFrame())
        {
            AttackProjectile projectile = PoolManager.GetObj<AttackProjectile>(ObjectPoolID.AttackProjectile_5_1);

            projectile.transform.SetParent(transform.parent);

            projectile.transform.position = offsetPosition.position;

            float angle = LookAtMouseUtils2D.GetLookAtMouseAngle(offsetPosition.position);

            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            projectile.Setting();
        }
    }
    public void Cancel()
    {
        offsetPosition.gameObject.SetActive(false);
        enabled = false;
    }
    private void OnEnable()
    {
        mousePositionInput = InputManager.GetInputAction(InputType.MousePoint);
        attackInput = InputManager.GetInputAction(InputType.FishingClick);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.MousePoint);
        InputManager.Release(InputType.FishingClick);
    }
}
