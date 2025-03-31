using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimer : MonoBehaviour
{
    [Header("플레이어 Flip 그룹")]
    [SerializeField] Transform flipTransform;

    [Header("플레이어 Flip 기준 위치")]
    [SerializeField] Transform pivotTransform;

    [SerializeField] List<PlayerWeapon> weaponList;

    InputActionReference mousePointInputActionReference;

    Vector3 normalScale = new Vector3(1, 1, 1);   // 플립되지 않은 크기  
    Vector3 flippedScale = new Vector3(-1, 1, 1); // 플립된 크기  

    public void Update()
    {

        #region Filp 수행

        Vector3 mousePosition = mousePointInputActionReference.action.ReadValue<Vector2>();

        Vector2 playerPosition = Camera.main.WorldToScreenPoint(pivotTransform.position);

        // TODO : flip 시켜놓는 방식 정하기
        if (mousePosition.x < playerPosition.x)
        {
            flipTransform.localScale = flippedScale;
        }
        else
        {
            flipTransform.localScale = normalScale;
        }

        #endregion

        #region 조준

        // 등록되어 있는 무기들을 모두 마우스 커서를 겨냥하게 만든다

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        bool isFlip = flipTransform.localScale.x < 0;

        foreach (PlayerWeapon weapon in weaponList)
        {
            weapon.SetDestination(mousePosition, isFlip);
        }

        #endregion

    }
    private void OnEnable()
    {
        mousePointInputActionReference = InputManager.GetInputAction(InputType.MousePoint);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.MousePoint);
    }
}
