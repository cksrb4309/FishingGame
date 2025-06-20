using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController_Game_1 : MonoBehaviour
{
    [SerializeField] Transform maskTransform;

    [SerializeField] SpriteRenderer cursor;

    InputActionReference clickInput = null;
    InputActionReference mousePositionInput = null;

    float attackCooldownTime = 1f;

    public void Setting()
    {
        cursor.gameObject.SetActive(true);
        enabled = true;
    }
    private void Update()
    {
        if (clickInput.action.WasPressedThisFrame())
        {
            if (attackCooldownTime <= 0f)
            {
                Attack();

                attackCooldownTime = FishingData.MiniGame_1_Data.AttackInterval;
            }
        }
        attackCooldownTime -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        cursor.transform.position = Camera.main.ScreenToWorldPoint(mousePositionInput.action.ReadValue<Vector2>());
    }
    public void Cancel()
    {
        cursor.gameObject.SetActive(false);
        enabled = false;
    }
    void Attack()
    {
        AttackAreaCircle attackArea = PoolManager.GetObj<AttackAreaCircle>(ObjectPoolID.AttackArea);
        attackArea.gameObject.SetActive(true);
        attackArea.transform.SetParent(maskTransform);
        attackArea.transform.localScale = Vector3.one;
        attackArea.transform.SetAsLastSibling();
        attackArea.Setting(cursor.transform.position);
    }
    private void OnEnable()
    {
        clickInput = InputManager.GetInputAction(InputType.FishingClick);
        mousePositionInput = InputManager.GetInputAction(InputType.MousePoint);

        cursor.size = Vector2.one * FishingData.MiniGame_1_Data.AttackRange * 2f;
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.FishingClick);
        InputManager.Release(InputType.MousePoint);
    }
}
