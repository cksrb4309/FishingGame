using System.Collections;
using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; } = null;

    IFishingSystem game = null;

    [SerializeField] Vector2 minBorder;
    [SerializeField] Vector2 maxBorder;

    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text maxHpText;

    [SerializeField] RectTransform myRect;

    Coroutine moveCoroutine = null;
    Coroutine rotateCoroutine = null;
    Vector2 direction = Vector2.zero;
    float speed = 0;
    int hp = 0;
    int rotateCount = 0;
    float rotateDelayHap = 0;
    float currentAngle = 0f;
    float targetAngle = 0f;
    float rotationSpeed = 360f; // degrees per second (적당히 조절 가능)
    float minRotateDelay = 0f;
    float maxRotateDelay = 0f;
    float balanceRotateDelay = 0f;
    int MaxHp
    {
        set => maxHpText.text = value.ToString();
    }
    int Hp
    {
        get => hp;
        set
        {
            hp = value;

            hpText.text = value.ToString();

            if (hp <= 0)
            {
                Complete();
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    public Vector2 Position => myRect.anchoredPosition;
    public void Attack(int damage)
    {
        Hp -= damage;
    }
    public void Setting(IFishingSystem game, FishingMethodData_Game_1 fishData, float hpMultiplier = 1f)
    {
        this.game = game;

        MaxHp = fishData.maxHp;
        Hp = (int)(fishData.maxHp * hpMultiplier);
        speed = fishData.speed;

        minRotateDelay = fishData.minRotateDelay;
        maxRotateDelay = fishData.maxRotateDelay;
        balanceRotateDelay = fishData.balanceRotateDelay;

        rotateCount = 0;
        rotateDelayHap = 0;

        // 초기 방향 및 회전값 설정
        float angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        // 초기 회전값 적용
        currentAngle = angle;
        targetAngle = angle;
        myRect.rotation = Quaternion.Euler(0, 0, angle);

        // 이동 및 회전 코루틴 시작
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

            Vector2 pos = myRect.anchoredPosition;
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

            myRect.anchoredPosition = nextPos;

            // 부드러운 회전
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            myRect.rotation = Quaternion.Euler(0, 0, currentAngle);
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

            rotateDelayHap += delay;
            rotateCount += 1;

            yield return new WaitForSeconds(delay);

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
    public void Cancel()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }
    public void Complete()
    {
        Cancel();

        game.Complete();
    }
}
