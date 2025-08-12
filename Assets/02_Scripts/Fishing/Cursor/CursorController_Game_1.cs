using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController_Game_1 : CursorController
{
    [SerializeField] Transform maskTransform;

    [SerializeField] SpriteRenderer cursor;

    void LateUpdate()
    {
        cursor.transform.position = Camera.main.ScreenToWorldPoint(mousePositionInput.action.ReadValue<Vector2>());
    }
    protected override void Shoot()
    {
        AttackAreaCircle attackArea = PoolManager.GetObj<AttackAreaCircle>(ObjectPoolID.AttackArea);
        attackArea.gameObject.SetActive(true);
        attackArea.transform.SetParent(maskTransform);
        attackArea.transform.localScale = Vector3.one;
        attackArea.transform.SetAsLastSibling();
        attackArea.Setting(cursor.transform.position);
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        cursor.size = Vector2.one * FishingData.MiniGame_1_Data.AttackRange * 2f;
    }
}
