using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class FishController_Horizontal : FishController
{
    [SerializeField] private SplineContainer spline;

    bool moveRight = true;

    int rotateCount = 0;

    float speed, t;
    float minRotateDelay = 0f, maxRotateDelay = 0f, balanceRotateDelay = 0f;
    float rotateDelayHap = 0f; 

    Coroutine moveCoroutine = null, rotateCoroutine = null;

    public override void Setting(FishingMethodData fishData, float hpMultiplier = 1)
    {
        base.Setting(fishData, hpMultiplier);

        FishingMethodData_Game_6 fishData_Game6 = fishData as FishingMethodData_Game_6;

        speed = fishData_Game6.speed;

        t = 0.43f;
        moveRight = true;

        minRotateDelay = fishData_Game6.minRotateDelay;
        maxRotateDelay = fishData_Game6.maxRotateDelay;
        balanceRotateDelay = fishData_Game6.balanceRotateDelay;

        rotateCount = 0;
        rotateDelayHap = 0;

        moveCoroutine = StartCoroutine(MoveCoroutine());
        rotateCoroutine = StartCoroutine(RotateCoroutine());
    }

    public override void Cancel()
    {
        base.Cancel();

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            float splineLength = spline.Spline.GetLength();

            float deltaT = (speed * Time.deltaTime) / splineLength;
            t += moveRight ? deltaT : -deltaT;

            // 끝에서 방향 반전
            if (t >= 1f) { t = 1f; moveRight = false; }
            else if (t <= 0f) { t = 0f; moveRight = true; }

            // 위치/회전
            Vector3 pos = (Vector3)spline.EvaluatePosition(t);
            Vector3 tangent = ((Vector3)spline.EvaluateTangent(t)).normalized;

            // 역방향이면 바라보는 방향 반전
            Vector3 lookDir = moveRight ? tangent : -tangent;

            // 2D 회전(Z축)
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            // z는 기존 유지(필요 시)
            pos.z = transform.position.z;

            transform.position = pos;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }
    }

    IEnumerator RotateCoroutine()
    {
        while (true)
        {
            //float delay =
            //    rotateDelayHap != 0 ?
            //    (rotateDelayHap / rotateCount) <= balanceRotateDelay ?
            //    Random.Range((balanceRotateDelay + minRotateDelay) * 0.5f, maxRotateDelay) :
            //    Random.Range(minRotateDelay, (balanceRotateDelay + maxRotateDelay) * 0.5f) :
            //    Random.Range(minRotateDelay, maxRotateDelay);

            float delay = Random.Range(minRotateDelay, maxRotateDelay);

            yield return new WaitForSeconds(delay);

            if (isStunned || !isAlive) continue;

            rotateDelayHap += delay;
            rotateCount += 1;

            moveRight ^= true;
        }
    }
}