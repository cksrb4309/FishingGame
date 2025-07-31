using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CursorController : MonoBehaviour
{
    public static CursorController Instance { get; private set; } = null;

    protected InputActionReference mousePositionInput = null;
    protected InputActionReference attackInput = null;

    bool isShoot = true;
    bool canShoot = true;

    public void Setting()
    {
        gameObject.SetActive(true);

        enabled = true;

        isShoot = true;
        canShoot = true;
    }
    public void SetMode(bool canShoot, bool isShoot = true)
    {
        this.canShoot = canShoot;

        this.isShoot = isShoot;
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
        enabled = false;
    }
    private void Update()
    {
        if (isShoot && attackInput.action.WasPressedThisFrame())
        {
            if (canShoot)
                Shoot();

            else
                Catch();
        }
    }
    void Catch()
    {
        FishController_New.Instance.ModifyCp(PlayerStat.Stat.catchGaugeGain);
    }
    protected virtual void OnEnable()
    {
        mousePositionInput = InputManager.GetInputAction(InputType.MousePoint);
        attackInput = InputManager.GetInputAction(InputType.FishingClick);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.MousePoint);
        InputManager.Release(InputType.FishingClick);
    }
    public static void CursorControllerRegister(CursorController controller) => Instance = controller;
    protected abstract void Shoot();
}
