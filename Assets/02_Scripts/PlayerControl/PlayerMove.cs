using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance { get; private set; } = null;

    [Header("가속")]
    [SerializeField] float defaultAcceleration;
    [SerializeField] float boostAcceleration;

    [Header("감속")]
    [SerializeField] float defaultDeaceleration;
    [SerializeField] float boostDeaceleration;

    [Header("최대 속도")]
    [SerializeField] float defaultMaxSpeed = 10f;
    [SerializeField] float boostMaxSpeed = 15f;
    [SerializeField] float maxSpeedTransitionSpeed = 3f;

    [Header("스태미나")]
    [SerializeField] float staminaRegenRate = 1f;
    [SerializeField] float staminaDrainRate = 1f;
    [SerializeField] float staminaRegenDelay = 0.1f;

    bool wasBoosting = false;
    bool canMove = true;

    float currentMaxSpeed;
    float currentStamina = 0f;
    float staminaRegenTime = 10f;
    float acceleration = 0f, deaceleration = 0f;

    Vector3 applyVelocity = Vector3.zero;
    Vector2 velocity = Vector2.zero;

    PlayerUIController playerUIController = null;

    Rigidbody2D myRigidbody;

    InputActionReference boostInputActionReference;
    InputActionReference leftInputActionReference;
    InputActionReference rightInputActionReference;
    InputActionReference upInputActionReference;
    InputActionReference downInputActionReference;

    Coroutine applyBoostMaxSpeedCoroutine = null;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerUIController = GetComponent<PlayerUIController>();

        currentMaxSpeed = defaultMaxSpeed;
        acceleration = defaultAcceleration;
        deaceleration = defaultDeaceleration;


        PlayerManager.SetPlayerTransform(transform);
    }
    private void Update()
    {
        #region 입력

        Vector2 moveValue = Vector2.zero;

        if (canMove)
        {
            moveValue.x += rightInputActionReference.action.IsPressed() ? 1f : 0f;
            moveValue.x += leftInputActionReference.action.IsPressed() ? -1f : 0f;
            moveValue.y += upInputActionReference.action.IsPressed() ? 1f : 0f;
            moveValue.y += downInputActionReference.action.IsPressed() ? -1f : 0f;
        }

        if (boostInputActionReference.action.IsPressed()) // 부스트 입력 시
        {
            if (currentStamina > 0f)
            {
                currentStamina -= Time.deltaTime * staminaDrainRate;

                if (currentStamina < 0f) currentStamina = 0f;

                playerUIController.SetStaminaAmount(currentStamina);

                staminaRegenTime = 0f;

                if (!wasBoosting) SetBoost_Local();
            }
            else if (currentStamina <= 0f)
            {
                currentStamina = 0f;

                if (wasBoosting) SetBoost_Local();
            }
        }
        else if (wasBoosting) SetBoost_Local();

        void SetBoost_Local()
        {
            wasBoosting = !wasBoosting;
            acceleration = wasBoosting ? boostAcceleration : defaultAcceleration;
            deaceleration = wasBoosting ? boostDeaceleration : defaultDeaceleration;
            SetBoostMaxSpeed(wasBoosting);
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

        #endregion

        #region 계산

        velocity += Time.deltaTime * acceleration * moveValue;

        if (Mathf.Abs(moveValue.x) <= 0.01f) SetDeacelerationMoveValue(ref velocity.x);

        if (Mathf.Abs(moveValue.y) <= 0.01f) SetDeacelerationMoveValue(ref velocity.y);

        void SetDeacelerationMoveValue(ref float value)
        {
            bool isPositive = value > 0f ? true : false;

            value += isPositive ? (Time.deltaTime * deaceleration * -1f) : (Time.deltaTime * deaceleration);

            if (isPositive) value = value > 0f ? value : 0f;

            else value = value < 0f ? value : 0f;
        }

        velocity = Vector2.ClampMagnitude(velocity, currentMaxSpeed);

        applyVelocity = velocity;

        #endregion

        #region 적용

        myRigidbody.linearVelocity = applyVelocity;

        #endregion
    }
    IEnumerator ApplyBoostMaxSpeedCoroutine(bool isBoost)
    {
        float targetSpeed = isBoost ? boostMaxSpeed : defaultMaxSpeed;

        while (true)
        {
            currentMaxSpeed += maxSpeedTransitionSpeed * (isBoost ? 1f : -1f) * Time.deltaTime;

            Debug.Log($"targetSpeed:{targetSpeed.ToString()} - currentMaxSpeed:{currentMaxSpeed.ToString()} > 0f");
            Debug.Log($"{(targetSpeed - currentMaxSpeed > 0f).ToString()} && {(!isBoost).ToString()}");

            if (!(targetSpeed - currentMaxSpeed > 0f ^ !isBoost))
            {
                currentMaxSpeed = targetSpeed; yield break;
            }
            yield return null;
        }
    }
    void SetBoostMaxSpeed(bool isBoost)
    {
        if (applyBoostMaxSpeedCoroutine != null) StopCoroutine(applyBoostMaxSpeedCoroutine);
        applyBoostMaxSpeedCoroutine = StartCoroutine(ApplyBoostMaxSpeedCoroutine(isBoost));
    }
    
    private void OnEnable()
    {
        boostInputActionReference = InputManager.GetInputAction(InputType.PlayerBoost);
        leftInputActionReference = InputManager.GetInputAction(InputType.Left);
        rightInputActionReference = InputManager.GetInputAction(InputType.Right);
        upInputActionReference = InputManager.GetInputAction(InputType.Up);
        downInputActionReference = InputManager.GetInputAction(InputType.Down);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.PlayerBoost);
        InputManager.Release(InputType.Left);
        InputManager.Release(InputType.Right);
        InputManager.Release(InputType.Up);
        InputManager.Release(InputType.Down);
    }
    public void EnableMove() => canMove = true;
    public void DisableMove() => canMove = false;
}
