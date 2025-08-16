using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishController_Free : FishController
{
    [SerializeField] Vector2 minBorder, maxBorder;

    Coroutine moveCoroutine = null, rotateCoroutine = null;

    Vector2 direction = Vector2.zero;

    int rotateCount = 0;
    float speed;
    float rotateDelayHap = 0f, currentAngle = 0f, targetAngle = 0f, rotationSpeed = 360f; // degrees per second (적당히 조절 가능)
    float minRotateDelay = 0f, maxRotateDelay = 0f, balanceRotateDelay = 0f;

    public override void Setting(FishingMethodData fishData, float hpMultiplier = 1f)
    {
        base.Setting(fishData, hpMultiplier);

        FishingMethodData_Game_5 fishData_Game5 = fishData as FishingMethodData_Game_5;

        speed = fishData_Game5.speed;

        lowHpCpMultiplier = fishData.lowHpCpMultiplier;

        minRotateDelay = fishData_Game5.minRotateDelay;
        maxRotateDelay = fishData_Game5.maxRotateDelay;
        balanceRotateDelay = fishData_Game5.balanceRotateDelay;

        rotateCount = 0;
        rotateDelayHap = 0;

        // 초기 방향 및 회전값 설정
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        // 초기 회전값 적용
        currentAngle = angle;
        targetAngle = angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);

        moveCoroutine = StartCoroutine(MoveCoroutine());
        rotateCoroutine = StartCoroutine(RotateCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return null;

            if (isStunned || !isAlive) continue;

            Vector2 pos = transform.localPosition;
            Vector2 nextPos = pos + direction * speed * Time.deltaTime;

            // X축 경계 반사
            if (nextPos.x < minBorder.x || nextPos.x > maxBorder.x)
            {
                direction.x *= -1;
                nextPos.x = Mathf.Clamp(nextPos.x, minBorder.x, maxBorder.x);
                targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }

            // Y축 경계 반사
            if (nextPos.y < minBorder.y || nextPos.y > maxBorder.y)
            {
                direction.y *= -1;
                nextPos.y = Mathf.Clamp(nextPos.y, minBorder.y, maxBorder.y);
                targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }

            transform.localPosition = nextPos;

            // 부드러운 회전
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }
    IEnumerator RotateCoroutine()
    {
        while (true)
        {
            float delay =
                rotateDelayHap != 0 ?
                (rotateDelayHap / rotateCount) <= balanceRotateDelay ?
                Random.Range((balanceRotateDelay + minRotateDelay) * 0.5f, maxRotateDelay) :
                Random.Range(minRotateDelay, (balanceRotateDelay + maxRotateDelay) * 0.5f) :
                Random.Range(minRotateDelay, maxRotateDelay);
                
            yield return new WaitForSeconds(delay);

            if (isStunned || !isAlive) continue;

            rotateDelayHap += delay;
            rotateCount += 1;

            Rotate();
        }
    }
    void Rotate()
    {
        // 목표 각도 설정
        targetAngle = Random.Range(0f, 360f);

        // 새 방향 계산
        direction = new Vector2(Mathf.Cos(targetAngle * Mathf.Deg2Rad), Mathf.Sin(targetAngle * Mathf.Deg2Rad)).normalized;
    }
    public override void Cancel()
    {
        base.Cancel();

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);

        if (stunHandle != null && stunHandle.IsActive() && stunHandle.IsPlaying()) stunHandle.Kill();
    }
}
