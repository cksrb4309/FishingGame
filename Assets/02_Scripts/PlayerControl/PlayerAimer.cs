using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimer : MonoBehaviour
{
    [Header("�÷��̾� Flip �׷�")]
    [SerializeField] Transform flipTransform;

    [Header("�÷��̾� Flip ���� ��ġ")]
    [SerializeField] Transform pivotTransform;

    [SerializeField] List<Transform> weaponTransforms = null;

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
        // TODO : ���� ������� ���ؿ� ���� �߰� ����� �ִٸ� WeaponAimSet Ŭ������ �����Ѵ�

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        foreach (var weaponTransform in weaponTransforms)
        {
            Vector3 dir = mousePosition - weaponTransform.position;

            float angle = (Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;

            Quaternion originalRotation = Quaternion.Euler(0, 0, angle);

            if (flipTransform.localScale.x < 0)
            
                originalRotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z + 180f);
            
            weaponTransform.rotation = originalRotation;
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
