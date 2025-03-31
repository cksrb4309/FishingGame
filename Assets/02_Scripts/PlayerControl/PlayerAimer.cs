using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimer : MonoBehaviour
{
    [Header("�÷��̾� Flip �׷�")]
    [SerializeField] Transform flipTransform;

    [Header("�÷��̾� Flip ���� ��ġ")]
    [SerializeField] Transform pivotTransform;

    [SerializeField] List<PlayerWeapon> weaponList;

    InputActionReference mousePointInputActionReference;

    Vector3 normalScale = new Vector3(1, 1, 1);   // �ø����� ���� ũ��  
    Vector3 flippedScale = new Vector3(-1, 1, 1); // �ø��� ũ��  

    public void Update()
    {

        #region Filp ����

        Vector3 mousePosition = mousePointInputActionReference.action.ReadValue<Vector2>();

        Vector2 playerPosition = Camera.main.WorldToScreenPoint(pivotTransform.position);

        // TODO : flip ���ѳ��� ��� ���ϱ�
        if (mousePosition.x < playerPosition.x)
        {
            flipTransform.localScale = flippedScale;
        }
        else
        {
            flipTransform.localScale = normalScale;
        }

        #endregion

        #region ����

        // ��ϵǾ� �ִ� ������� ��� ���콺 Ŀ���� �ܳ��ϰ� �����

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
