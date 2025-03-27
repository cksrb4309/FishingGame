using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform collisionMarkers;

    [SerializeField] float acceleration;
    [SerializeField] float boostAcceleration;

    [SerializeField] float deceleration;
    [SerializeField] float boostDeceleration;
    [SerializeField] float limitedSpeed = 1f;

    [SerializeField] float maxSpeed;
    [SerializeField] float boostMaxSpeed;

    [SerializeField] float dashSpeed;
    [SerializeField] float doubleClickLimit = 0.2f;
    [SerializeField] float dashCooltime = 1f;

    [SerializeField] float bounceFactor = 1f;
    [SerializeField] float minBounceForce = 0.5f;

    //bool canMove = true;

    [SerializeField] Vector2 velocity = Vector2.zero;
    Vector3 applyVelocity = Vector3.zero;

    Transform myTransform;

    List<Transform>[] dirMarkers = new List<Transform>[4];
    List<float>[] weightList = new List<float>[4];

    InputActionReference moveInputActionReference;
    InputActionReference boostInputActionReference;
    InputActionReference leftInputActionReference;
    InputActionReference rightInputActionReference;

    public void HandleCollision(Collision2D collision)
    {

        #region 방향 계산

        int contactCount = collision.contactCount;

        InitWeight();

        float maxWeight = 0f;
        Dir selectDir = Dir.Top;

        for (int i = 0; i < contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i); // 각 충돌 지점 가져오기

            Vector2 collisionPoint = contact.point; // 충돌 지점

            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < dirMarkers[j].Count; k++)
                {
                    Vector2 pos = dirMarkers[j][k].position;

                    weightList[j][k] += Vector2.Distance(collisionPoint, pos);

                    if (maxWeight < weightList[j][k])
                    {
                        Debug.Log("최대값 갱신 값 : " + weightList[j][k].ToString() + " / 방향 : " + ((Dir)j).ToString());

                        selectDir = (Dir)j;

                        maxWeight = weightList[j][k];
                    }
                }
            }
        }

        #endregion

        #region 힘 적용

        float value;

        switch (selectDir)
        {
            case Dir.Top:
                value = -velocity.y * bounceFactor;
                value = value < minBounceForce ? minBounceForce : value;
                velocity.y = value; break;
            case Dir.Bottom:
                value = -velocity.y * bounceFactor;
                value = value > -minBounceForce ? -minBounceForce : value;
                velocity.y = value; break;
            case Dir.Left:
                value = -velocity.x * bounceFactor;
                value = value > -minBounceForce ? -minBounceForce : value;
                velocity.x = value; break;
            case Dir.Right:
                value = -velocity.x * bounceFactor;
                value = value < minBounceForce ? minBounceForce : value;
                velocity.x = value; break;
        }

        #endregion

    }
    void InitWeight()
    {
        for (int i = 0; i < 4; i++) for (int j = 0; j < weightList[i].Count; j++) weightList[i][j] = 0f;
    }

    Dir beforeMoveDir = Dir.Top;
    Coroutine dashCoroutine = null;
    Coroutine dashCooltimeCoroutine = null;
    IEnumerator DashCoroutine()
    {
        yield return new WaitForSecondsRealtime(doubleClickLimit);

        dashCoroutine = null;
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

        #region 대쉬 적용 입력

        if (dashCoroutine != null && dashCooltimeCoroutine == null)
        {
            if (leftInputActionReference.action.WasPressedThisFrame())
            {
                if (beforeMoveDir == Dir.Left)
                {
                    StopCoroutine(dashCoroutine);
                    dashCoroutine = null;
                    velocity.x = -dashSpeed;

                    dashCooltimeCoroutine = StartCoroutine(DashCooltimeCoroutine());
                }
            }
            else if (rightInputActionReference.action.WasPressedThisFrame())
            {
                if (beforeMoveDir == Dir.Right)
                {
                    StopCoroutine(dashCoroutine);
                    dashCoroutine = null;
                    velocity.x = dashSpeed;

                    dashCooltimeCoroutine = StartCoroutine(DashCooltimeCoroutine());
                }
            }
        }
        else
        {
            if (leftInputActionReference.action.WasPressedThisFrame())
            {
                beforeMoveDir = Dir.Left;
                dashCoroutine = StartCoroutine(DashCoroutine());
            }
            else if (rightInputActionReference.action.WasPressedThisFrame())
            {
                beforeMoveDir = Dir.Right;
                dashCoroutine = StartCoroutine(DashCoroutine());
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

            velocity.x += isPositive ? (Time.deltaTime * (isBoost ? boostDeceleration : deceleration) * -1f) : (Time.deltaTime * (isBoost ? boostDeceleration : deceleration));

            if (isPositive) velocity.x = velocity.x > 0f ? velocity.x : 0f;

            else velocity.x = velocity.x < 0f ? velocity.x : 0f;
        }

        if (Mathf.Abs(moveValue.y) <= 0.01f)
        {
            isPositive = velocity.y > 0f ? true : false;

            velocity.y += isPositive ? (Time.deltaTime * -(isBoost ? boostDeceleration : deceleration)) : (Time.deltaTime * (isBoost ? boostDeceleration : deceleration));

            if (isPositive) velocity.y = velocity.y < 0f ? 0f : velocity.y;

            else velocity.y = velocity.y > 0f ? 0f : velocity.y;
        }

        Vector2 beforeVelocity = velocity;
        Vector2 afterVelocity = Vector2.ClampMagnitude(velocity, isBoost ? boostMaxSpeed : maxSpeed);

        velocity = Vector2.Lerp(beforeVelocity, afterVelocity, Time.deltaTime * limitedSpeed);

        applyVelocity = velocity;

        #endregion

        #region 적용

        myTransform.position += applyVelocity * Time.deltaTime;

        #endregion

    }
    private void Awake()
    {
        myTransform = GetComponent<Transform>();

        // 충돌 방향 마커 초기화
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
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable");

        moveInputActionReference = InputManager.GetInputAction(InputType.PlayerMove);
        boostInputActionReference = InputManager.GetInputAction(InputType.PlayerBoost);
        leftInputActionReference = InputManager.GetInputAction(InputType.PlayerLeft);
        rightInputActionReference = InputManager.GetInputAction(InputType.PlayerRight);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.PlayerMove);
        InputManager.Release(InputType.PlayerBoost);
        InputManager.Release(InputType.PlayerLeft);
        InputManager.Release(InputType.PlayerRight);
    }
    //public void Activate() => canMove = true;
    //public void Deactivate() => canMove = false;
}

public enum Dir
{
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3,
}