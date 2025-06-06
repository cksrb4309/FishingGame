using System.Collections;
using TMPro;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; } = null;

    [SerializeField] Fishing_Game_1 game;

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
    float angle = 0f;
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
    public void Setting(FishingMethodData_Game_1 fishData, float hpMultiplier = 1f)
    {
        MaxHp = fishData.maxHp;
        Hp = (int)(fishData.maxHp * hpMultiplier);
        speed = fishData.speed;

        minRotateDelay = fishData.minRotateDelay;
        maxRotateDelay = fishData.maxRotateDelay;
        balanceRotateDelay = fishData.balanceRotateDelay;

        rotateCount = 0;
        rotateDelayHap = 0;
        angle = Random.Range(0f, 360f);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
        myRect.rotation = Quaternion.Euler(0, 0, angle);

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
            }

            // Y축 경계 반사
            if (nextPos.y < minBorder.y || nextPos.y > maxBorder.y)
            {
                direction.y *= -1;
                nextPos.y = Mathf.Clamp(nextPos.y, minBorder.y, maxBorder.y);
            }

            myRect.anchoredPosition = nextPos;

            // 회전 업데이트
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            myRect.rotation = Quaternion.Euler(0, 0, angle);
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
        // 랜덤 각도 설정
        angle = Random.Range(0f, 360f);

        // 방향 벡터 갱신
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

        // 회전 적용 (UI 시각적 표현)
        myRect.rotation = Quaternion.Euler(0f, 0f, angle);
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
