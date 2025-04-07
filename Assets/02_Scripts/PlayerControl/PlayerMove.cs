using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerMove : MonoBehaviour
{
    [Header("충돌 위치")]
    [SerializeField] Transform collisionMarkers;

    [Header("가속")]
    [SerializeField] float acceleration;
    [SerializeField] float boostAcceleration;

    [Header("감속")]
    [SerializeField] float deaceleration;
    [SerializeField] float boostDeaceleration;

    [Header("최대 속도")]
    [SerializeField] float defaultMaxSpeed = 10f;
    [SerializeField] float boostMaxSpeed = 15f;

    [Header("제한 값 적용 속도")]
    [SerializeField] float limitedSpeed = 1f;

    [Header("대쉬")]
    [SerializeField] AnimationCurve dashSpeed;
    [SerializeField] float dashFinishSpeed = 1.5f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float dashCooltime = 1f;

    [Header("탄성")]
    [SerializeField] float bounceFactor = 1f;
    [SerializeField] float minBounceForce = 0.5f;

    [Header("스태미나")]
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

    Transform[] dirMarkers;
    float[] weights;

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

        #region 방향 계산

        int contactCount = collision.contactCount;

        for (int i = 0; i < weights.Length; i++) weights[i] = 0f;

        float maxWeight = 0f;
        Transform selectPosition = null;

        for (int i = 0; i < contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i); // 각 충돌 지점 가져오기

            Vector2 collisionPoint = contact.point; // 충돌 지점

            for (int j = 0; j < dirMarkers.Length; j++)
            {
                Vector2 pos = dirMarkers[j].position;

                weights[j] += Vector2.Distance(collisionPoint, pos);

                if (maxWeight < weights[j])
                {
                    selectPosition = dirMarkers[j];

                    maxWeight = weights[j];
                }
            }
        }

        #endregion

        #region 힘 적용

        Vector2 velocity = isDash ? dashVelocity : this.velocity;

        Vector3 dir = (transform.position - selectPosition.position).normalized; // 튕겨나갈 방향을 나타내는 벡터

        // dir의 방향을 기반으로 회전 없이 가장 가까운 4방향으로 변환
        Vector2 normalizedDir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // x가 더 크면 left 또는 right
        {
            normalizedDir = (dir.x > 0) ? Vector2.right : Vector2.left;
        }
        else // y가 더 크면 up 또는 down
        {
            normalizedDir = (dir.y > 0) ? Vector2.up : Vector2.down;
        }

        // 벡터 방향에 따라 반사 벡터를 계산
        Vector3 bounce = Vector3.Reflect(velocity, normalizedDir) * bounceFactor;

        // 최소 튕김 힘 보장
        if (Mathf.Abs(bounce.x) < minBounceForce && bounce.x != 0f)
            bounce.x = Mathf.Sign(bounce.x) * minBounceForce;

        if (Mathf.Abs(bounce.y) < minBounceForce && bounce.y != 0f)
            bounce.y = Mathf.Sign(bounce.y) * minBounceForce;

        this.velocity = bounce;

        // 반사된 벡터(bounce)의 방향을 적용하도록 수정
        Vector3 bounceDirection = bounce.normalized;  // 반사된 벡터의 정규화된 방향

        // 튕겨나가는 방향을 제대로 디버깅
        Debug.Log("Bounce Dir : " + bounceDirection);  // 반사된 벡터의 방향

        #endregion

        if (isDash) isDash = false;
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

        #region 입력

        Vector2 moveValue = moveInputActionReference.action.ReadValue<Vector2>();

        bool isBoost = boostInputActionReference.action.IsPressed();

        if (isBoost) // 부스트 입력 시
        {
            if (currentStamina > 0f)
            {
                currentStamina -= Time.deltaTime * (isDash ? 0f : staminaDrainRate);

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

        #region 대쉬 적용 입력

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

        #region 계산

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

        #region 적용

        myRigidbody.linearVelocity = applyVelocity;

        //myTransform.position += applyVelocity * Time.deltaTime;

        #endregion

    }
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        dirMarkers = new Transform[collisionMarkers.childCount];
        weights = new float[collisionMarkers.childCount];

        int index = 0;

        foreach (Transform collisionMarker in collisionMarkers)
        {
            dirMarkers[index] = collisionMarker;
            weights[index++] = 0f;
        }

        playerUIController = GetComponent<PlayerUIController>();

        currentStamina = 1f;

        NormalizeCurve(dashSpeed, dashDuration);

        PlayerManager.SetPlayerTransform(transform);
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
        float originalDuration = keys[keys.Length - 1].time; // 원래 커브의 마지막 키프레임 시간

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