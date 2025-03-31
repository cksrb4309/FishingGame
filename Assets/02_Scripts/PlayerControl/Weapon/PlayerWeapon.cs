using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("무기 회전 축 Transform")]
    [SerializeField] Transform[] pivotTransforms;

    [Header("무기 총구 Transform")]
    [SerializeField] Transform[] fireTransforms;

    [Header("에임 겨누는 속도")]
    [SerializeField] float aimSpeed; // 조준 속도

    [Header("공격 간격")]
    [SerializeField] float attackInterval = 1f;

    [Header("투사체 프리팹")]
    [SerializeField] Bullet bulletPrefab;

    float[] currentAngles = new float[2];

    float timeSinceLastShot = 10f;

    bool beforeFlip = false;

    InputActionReference fireInputActionReference;

    private void Update()
    {
        bool isPressed = fireInputActionReference.action.IsPressed();

        timeSinceLastShot += Time.deltaTime;

        if (isPressed && timeSinceLastShot >= attackInterval)
        {
            timeSinceLastShot = 0f;

            Fire();
        }
    }
    void Fire()
    {
        foreach (Transform fireTransform in fireTransforms)
        {
            Bullet bullet = PoolManager.GetObj<Bullet>(ObjectPoolID.Bullet);

            bullet.transform.position = fireTransform.position;

            bullet.Setting(fireTransform.eulerAngles.z + (beforeFlip ? 180f : 0f));
        }
    }
    public void SetDestination(Vector3 position, bool isFlip)
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 dir = position - pivotTransforms[i].position;
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            // 현재 각도를 초당 aimSpeed만큼 변경
            currentAngles[i] = Mathf.MoveTowardsAngle(currentAngles[i], targetAngle, aimSpeed * Time.deltaTime);

            float applyAngle = currentAngles[i];

            if (isFlip) applyAngle += 180f;

            pivotTransforms[i].rotation = Quaternion.Euler(0, 0, applyAngle);
        }

        void FlipSet()
        {
            beforeFlip = !beforeFlip;

            (currentAngles[0], currentAngles[1]) = (currentAngles[1], currentAngles[0]);

            for (int i = 0; i < 2; i++)
            {
                float applyAngle = currentAngles[i];

                if (isFlip) applyAngle += 180f;

                pivotTransforms[i].rotation = Quaternion.Euler(0, 0, applyAngle);
            }
        }

        if (isFlip && !beforeFlip)
        {
            FlipSet();
        }
        else if (!isFlip && beforeFlip)
        {
            FlipSet();
        }
    }
    private void Awake()
    {
        PoolManager.CreatePool(ObjectPoolID.Bullet, bulletPrefab);
    }
    private void OnEnable()
    {
        fireInputActionReference = InputManager.GetInputAction(InputType.PlayerAttack);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.PlayerAttack);
    }
}
