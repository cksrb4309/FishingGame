using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("�浹 ��ġ")]
    [SerializeField] Transform collisionMarkers;

    [Header("����")]
    [SerializeField] float acceleration;
    [SerializeField] float boostAcceleration;

    [Header("����")]
    [SerializeField] float deaceleration;
    [SerializeField] float boostDeaceleration;

    [Header("�ִ� �ӵ�")]
    [SerializeField] float defaultMaxSpeed = 10f;
    [SerializeField] float boostMaxSpeed = 15f;

    [Header("���� �� ���� �ӵ�")]
    [SerializeField] float limitedSpeed = 1f;

    [Header("�뽬")]
    [SerializeField] AnimationCurve dashSpeed;
    [SerializeField] float dashFinishSpeed = 1.5f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashCooltime = 1f;

    [Header("ź��")]
    [SerializeField] float bounceFactor = 1f;
    [SerializeField] float minBounceForce = 0.5f;

    [Header("���¹̳�")]
    [SerializeField] float staminaRegenRate = 1f;
    [SerializeField] float staminaDrainRate = 1f;
    [SerializeField] float staminaRegenDelay = 0.1f;

    //bool canMove = true;


    bool isDash = false;

    float currentStamina = 0f;
    float staminaRegenTime = 10f;

    Vector3 applyVelocity = Vector3.zero;

    Vector2 velocity = Vector2.zero;
    Vector2 dashVelocity = Vector2.zero;

    PlayerUIController playerUIController = null;

    Rigidbody2D myRigidbody;

    List<Transform>[] dirMarkers = new List<Transform>[4];
    List<float>[] weightList = new List<float>[4];

    Coroutine dashCooltimeCoroutine = null;

    InputActionReference moveInputActionReference;
    InputActionReference boostInputActionReference;
    InputActionReference dashInputActionReference;
    //InputActionReference mousePointInputActionReference;
    InputActionReference leftInputActionReference;
    InputActionReference rightInputActionReference;
    InputActionReference upInputActionReference;
    InputActionReference downInputActionReference;

    public void HandleCollision(Collision2D collision)
    {

        #region ���� ���

        int contactCount = collision.contactCount;

        InitWeight();

        float maxWeight = 0f;
        Dir selectDir = Dir.Top;

        for (int i = 0; i < contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i); // �� �浹 ���� ��������

            Vector2 collisionPoint = contact.point; // �浹 ����

            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < dirMarkers[j].Count; k++)
                {
                    Vector2 pos = dirMarkers[j][k].position;

                    weightList[j][k] += Vector2.Distance(collisionPoint, pos);

                    if (maxWeight < weightList[j][k])
                    {
                        selectDir = (Dir)j;

                        maxWeight = weightList[j][k];
                    }
                }
            }
        }

        #endregion

        #region �� ����

        float value;

        Vector2 velocity = isDash ? dashVelocity : this.velocity;

        switch (selectDir)
        {
            case Dir.Top:
                value = -velocity.y * bounceFactor;
                value = value < minBounceForce ? minBounceForce : value;
                this.velocity.y = value; break;
            case Dir.Bottom:
                value = -velocity.y * bounceFactor;
                value = value > -minBounceForce ? -minBounceForce : value;
                this.velocity.y = value; break;
            case Dir.Left:
                value = -velocity.x * bounceFactor;
                value = value > -minBounceForce ? -minBounceForce : value;
                this.velocity.x = value; break;
            case Dir.Right:
                value = -velocity.x * bounceFactor;
                value = value < minBounceForce ? minBounceForce : value;
                this.velocity.x = value; break;
        }

        #endregion

        if (isDash) isDash = false;
    }
    void InitWeight()
    {
        for (int i = 0; i < 4; i++) for (int j = 0; j < weightList[i].Count; j++) weightList[i][j] = 0f;
    }
    IEnumerator DashCoroutine(Vector2 dir)
    {
        //Vector2 mousePosition = mousePointInputActionReference.action.ReadValue<Vector2>();
        //Vector2 dir = (mousePosition - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f)).normalized;

        dir = dir.normalized;

        float t = 0;

        isDash = true;

        while (t < dashDuration)
        {
            t += Time.deltaTime;

            dashVelocity = dir * dashSpeed.Evaluate(t);

            yield return null;
        }

        if (isDash)
        {
            dir = Vector2.zero;

            if (leftInputActionReference.action.IsPressed()) dir.x -= 1f;
            if (rightInputActionReference.action.IsPressed()) dir.x += 1f;
            if (upInputActionReference.action.IsPressed()) dir.y += 1f;
            if (downInputActionReference.action.IsPressed()) dir.y -= 1f;

            dir = dir.normalized;

            velocity = dir * defaultMaxSpeed * dashFinishSpeed;
        }

        isDash = false;
    }
    IEnumerator DashCooltimeCoroutine()
    {
        yield return new WaitForSeconds(dashCooltime);

        dashCooltimeCoroutine = null;
    }
    private void Update()
    {

        #region �Է�

        Vector2 moveValue = moveInputActionReference.action.ReadValue<Vector2>();

        bool isBoost = boostInputActionReference.action.IsPressed();

        if (isBoost) // ��� �Է� ��
        {
            if (currentStamina > 0f)
            {
                currentStamina -= Time.deltaTime * staminaDrainRate;

                if (currentStamina < 0f) currentStamina = 0f;

                playerUIController.SetStaminaAmount(currentStamina);

                staminaRegenTime = 0f;
            }
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;

                isBoost = false;
            }
        }

        staminaRegenTime += Time.deltaTime;

        if (staminaRegenTime >= staminaRegenDelay)
        {
            if (currentStamina < 1f)
            {
                currentStamina += Time.deltaTime * staminaRegenRate;

                if (currentStamina > 1f) currentStamina = 1f;

                playerUIController.SetStaminaAmount(currentStamina);
            }
        }

        #region �뽬 ���� �Է�

        if (dashCooltimeCoroutine == null)
        {
            if (dashInputActionReference.action.WasPressedThisFrame())
            {
                Vector2 dir = Vector2.zero;

                if (leftInputActionReference.action.IsPressed()) dir.x -= 1f;
                if (rightInputActionReference.action.IsPressed()) dir.x += 1f;
                if (upInputActionReference.action.IsPressed()) dir.y += 1f;
                if (downInputActionReference.action.IsPressed()) dir.y -= 1f;

                if (Vector2.zero != dir)
                {
                    StartCoroutine(DashCoroutine(dir));

                    dashCooltimeCoroutine = StartCoroutine(DashCooltimeCoroutine());
                }
            }
        }

        #endregion

        #endregion

        #region ���

        velocity += (Time.deltaTime * (isBoost ? boostAcceleration : acceleration)) * moveValue;

        bool isPositive;

        if (Mathf.Abs(moveValue.x) <= 0.01f)
        {
            isPositive = velocity.x > 0f ? true : false;

            velocity.x += isPositive ? (Time.deltaTime * (isBoost ? boostDeaceleration : deaceleration) * -1f) : (Time.deltaTime * (isBoost ? boostDeaceleration : deaceleration));

            if (isPositive) velocity.x = velocity.x > 0f ? velocity.x : 0f;

            else velocity.x = velocity.x < 0f ? velocity.x : 0f;
        }

        if (Mathf.Abs(moveValue.y) <= 0.01f)
        {
            isPositive = velocity.y > 0f ? true : false;

            velocity.y += isPositive ? (Time.deltaTime * -(isBoost ? boostDeaceleration : deaceleration)) : (Time.deltaTime * (isBoost ? boostDeaceleration : deaceleration));

            if (isPositive) velocity.y = velocity.y < 0f ? 0f : velocity.y;

            else velocity.y = velocity.y > 0f ? 0f : velocity.y;
        }

        Vector2 beforeVelocity = velocity;
        Vector2 afterVelocity = Vector2.ClampMagnitude(velocity, isBoost ? boostMaxSpeed : defaultMaxSpeed);

        velocity = Vector2.Lerp(beforeVelocity, afterVelocity, Time.deltaTime * limitedSpeed);

        applyVelocity = isDash ? dashVelocity : velocity;

        #endregion

        #region ����

        myRigidbody.linearVelocity = applyVelocity;

        //myTransform.position += applyVelocity * Time.deltaTime;

        #endregion

    }
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        // �浹 ���� ��Ŀ �ʱ�ȭ
        for (int i = 0; i < 4; i++)
        {
            dirMarkers[i] = new List<Transform>();
            weightList[i] = new List<float>();

            for (int j = 0; j < collisionMarkers.GetChild(i).childCount; j++)
            {
                dirMarkers[i].Add(collisionMarkers.GetChild(i).GetChild(j));
                weightList[i].Add(0f);
            }
        }
        playerUIController = GetComponent<PlayerUIController>();

        currentStamina = 1f;

        NormalizeCurve(dashSpeed, dashDuration);
    }
    private void OnEnable()
    {
        moveInputActionReference = InputManager.GetInputAction(InputType.PlayerMove);
        boostInputActionReference = InputManager.GetInputAction(InputType.PlayerBoost);
        dashInputActionReference = InputManager.GetInputAction(InputType.PlayerDash);
        //mousePointInputActionReference = InputManager.GetInputAction(InputType.MousePoint);

        leftInputActionReference = InputManager.GetInputAction(InputType.Left);
        rightInputActionReference = InputManager.GetInputAction(InputType.Right);
        upInputActionReference = InputManager.GetInputAction(InputType.Up);
        downInputActionReference = InputManager.GetInputAction(InputType.Down);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.PlayerMove);
        InputManager.Release(InputType.PlayerBoost);
        InputManager.Release(InputType.PlayerDash);
        //InputManager.Release(InputType.MousePoint);

        InputManager.Release(InputType.Left);
        InputManager.Release(InputType.Right);
        InputManager.Release(InputType.Up);
        InputManager.Release(InputType.Down);
    }
    //public void Activate() => canMove = true;
    //public void Deactivate() => canMove = false;

    AnimationCurve NormalizeCurve(AnimationCurve curve, float dashDuration)
    {
        if (curve == null || curve.keys.Length == 0)
            return new AnimationCurve();

        AnimationCurve normalizedCurve = new AnimationCurve();

        Keyframe[] keys = curve.keys;
        float originalDuration = keys[keys.Length - 1].time; // ���� Ŀ���� ������ Ű������ �ð�

        foreach (Keyframe key in keys)
        {
            float normalizedTime = (key.time / originalDuration) * dashDuration;
            normalizedCurve.AddKey(new Keyframe(normalizedTime, key.value, key.inTangent, key.outTangent));
        }

        return normalizedCurve;
    }
}

public enum Dir
{
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3,
}