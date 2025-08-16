using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; } = null;

    protected InputActionReference mousePositionInput = null;
    protected InputActionReference attackInput = null;

    protected bool isShoot = true;
    protected bool canShoot = true;

    public virtual void Setting()
    {
        gameObject.SetActive(true);

        enabled = true;

        isShoot = true;
        canShoot = true;
    }
    public virtual void SetMode(bool canShoot, bool isShoot = true)
    {
        this.canShoot = canShoot;

        this.isShoot = isShoot;
    }
    public virtual void Cancel()
    {
        gameObject.SetActive(false);
        enabled = false;
    }
    protected virtual void Update()
    {
        if (isShoot && attackInput.action.WasPressedThisFrame())
        {
            if (canShoot)
            {
                isShoot = false;

                Shoot();

                SetAttackCooltime();
            }

            else Catch();
        }
    }
    protected virtual void Catch()
    {
        FishController.Instance.ModifyCp(PlayerStat.Stat.catchGaugeGain);
    }
    protected virtual void OnEnable()
    {
        mousePositionInput = InputManager.GetInputAction(InputType.MousePoint);
        attackInput = InputManager.GetInputAction(InputType.FishingClick);
    }
    protected virtual void OnDisable()
    {
        InputManager.Release(InputType.MousePoint);
        InputManager.Release(InputType.FishingClick);
    }
    public static void CursorControllerRegister(CursorController controller) => Instance = controller;
    protected abstract void Shoot();
    protected abstract void SetAttackCooltime();
}
