using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingCursorController : MonoBehaviour
{
    [SerializeField] Transform maskTransform;

    [SerializeField] Image cursorImage;
    [SerializeField] Sprite game_1_Cursor;

    InputActionReference clickInput = null;
    InputActionReference mousePositionInput = null;

    float attackCooldownTime = 1f;

    public void Game_1_Setting()
    {
        cursorImage.gameObject.SetActive(true);
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
        cursorImage.transform.position = Camera.main.ScreenToWorldPoint(mousePositionInput.action.ReadValue<Vector2>());
    }
    public void Cancel()
    {
        cursorImage.gameObject.SetActive(false);
        enabled = false;
    }
    void Attack()
    {
        AttackArea attackArea = PoolManager.GetObj<AttackArea>(ObjectPoolID.AttackArea);
        attackArea.gameObject.SetActive(true);
        attackArea.transform.SetParent(maskTransform);
        attackArea.transform.localScale = Vector3.one;
        attackArea.transform.SetAsLastSibling();
        attackArea.Setting(cursorImage.rectTransform.anchoredPosition);
    }
    private void OnEnable()
    {
        clickInput = InputManager.GetInputAction(InputType.FishingClick);
        mousePositionInput = InputManager.GetInputAction(InputType.MousePoint);

        cursorImage.rectTransform.sizeDelta = Vector2.one * FishingData.MiniGame_1_Data.AttackRange * 2f;
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.FishingClick);
        InputManager.Release(InputType.MousePoint);
    }
}
