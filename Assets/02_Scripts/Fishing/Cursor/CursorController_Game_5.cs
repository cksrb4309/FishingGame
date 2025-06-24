using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController_Game_5 : MonoBehaviour
{
    public static CursorController_Game_5 Instance { get; private set; } = null;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform offsetPosition;

    InputActionReference mousePositionInput = null;
    InputActionReference attackInput = null;

    bool isShoot = true;
    bool canShoot = true;
    bool canCatch = false;
    public void Setting()
    {
        gameObject.SetActive(true);

        isShoot = true;

        canShoot = true;
        canCatch = false;

        enabled = true;
    }
    private void Update()
    {
        if (isShoot && attackInput.action.WasPressedThisFrame())
        {
            if (canShoot) Shoot();

            if (canCatch) Catch();
        }
    }
    void Shoot()
    {
        AttackProjectile_New projectile = PoolManager.GetObj<AttackProjectile_New>(ObjectPoolID.AttackProjectile_5_1);

            projectile.transform.SetParent(transform.parent);

            projectile.transform.position = offsetPosition.position;

            Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(mousePositionInput.action.ReadValue<Vector2>());
            targetWorldPos.z = offsetPosition.position.z;
            Vector3 dir = targetWorldPos - offsetPosition.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

            projectile.Setting();
    }
    void Catch()
    {
        FishController_New.Instance.ModifyCp(FishingData.MiniGame_5_Data.AttackCp);
    }
    public void SetMode(bool canShoot, bool isShoot = true)
    {
        this.canShoot = canShoot;
        canCatch = !canShoot;

        this.isShoot = isShoot;
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
    void Awake()
    {
        Instance = this;

        PoolManager.CreatePool<AttackProjectile_New>(ObjectPoolID.AttackProjectile_5_1, 5);
    }
}
