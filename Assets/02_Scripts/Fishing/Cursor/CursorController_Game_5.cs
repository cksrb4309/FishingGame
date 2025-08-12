using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController_Game_5 : CursorController
{
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform offsetPosition;

    protected override void Shoot()
    {
        AttackProjectile_New projectile = PoolManager.GetObj<AttackProjectile_New>(ObjectPoolID.AttackProjectile_5_1);

        projectile.transform.SetParent(transform.parent);

        projectile.transform.position = offsetPosition.position;

        float angle = LookAtMouseUtils2D.GetLookAtMouseAngle(Camera.main.WorldToScreenPoint(offsetPosition.position));

        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        projectile.Setting();
    }
}
