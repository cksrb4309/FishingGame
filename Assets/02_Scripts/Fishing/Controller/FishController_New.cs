using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishController_New : MonoBehaviour
{
  public static FishController_New Instance { get; private set; } = null;

    [SerializeField] Vector2 minBorder, maxBorder;

    [SerializeField] TMP_Text hpText, spText, cpText;

    [SerializeField] Image hpFillImage, spFillImage, cpFillImage;

    Coroutine moveCoroutine = null, rotateCoroutine = null, completeCoroutine = null;

    Vector2 direction = Vector2.zero;

    Tween stunHandle = null;

    bool isAlive, isStunned;

    int maxHp, hp, maxSp, sp;
    float cp, maxCp, speed;
    int rotateCount = 0;
    float rotateDelayHap = 0f, currentAngle = 0f, targetAngle = 0f, rotationSpeed = 360f; // degrees per second (적당히 조절 가능)
    float minRotateDelay = 0f, maxRotateDelay = 0f, balanceRotateDelay = 0f, lowHpCpMultiplier = 0f;

    #region Hp, Sp, Cp 속성
    int Hp
    {
        get => hp;
        set
        {
            hp = value;

            if (value > 0)
            {
                hpText.text = (((float)hp / maxHp) * 100).ToString("F0") + '%';
                hpFillImage.fillAmount = (float)hp / maxHp;
            }

            else
            {
                hpText.text = "0%";
                hpFillImage.fillAmount = 0;
            }

            if (hp <= 0 && isAlive) Die();
        }
    }
    int Sp
    {
        get => sp;
        set
        {
            sp = value;

            if (sp > 0)
            {
                spText.text = (((float)sp / maxSp) * 100).ToString("F0") + '%';
                spFillImage.fillAmount = (float)sp / maxSp;
            }

            else
            {
                spText.text = "0%";
                spFillImage.fillAmount = 0;
            }

            if (sp <= 0 && !isStunned) Stun();
        }
    }
    float Cp
    {
        get => cp;
        set
        {
            cp = value;
            
            if (cp > 0f && cp < maxCp)
            {
                cpText.text = ((cp / maxCp) * 100).ToString("F0") + '%';
                cpFillImage.fillAmount = cp / maxCp;
            }

            else
            {
                cpText.text = cp <= 0f ? "0%" : "100%";
                cpFillImage.fillAmount = cp <= 0f ? 0 : 1;
            }

            if (cp <= 0) Cancel();
            
            if (cp >= maxCp) Complete();
        }
    }
    #endregion
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    public Vector2 WorldPosition => transform.position;
    public Vector2 LocalPosition => transform.localPosition;

    public void ModifyHp(int amount)
    {
        if (!isAlive) return;

        if (amount < 0) 
        {
            // 현재 HP 비율 계산 (0~1)
            float currentHpRatio = (float)hp / maxHp;
            
            // HP가 최대(1.0)일 때는 그대로, 최소(0.0)일 때는 LowHpCpMultiplier 적용
            float damageMultiplier = Mathf.Lerp(lowHpCpMultiplier, 1f, currentHpRatio);
            
            amount = Mathf.RoundToInt(amount * damageMultiplier);
        }

        Hp += amount;
    }
    public void ModifySp(int amount)
    {
        if (!isAlive) return;

        Sp += amount;
    }
    public void ModifyCp(float amount)
    {
        Cp += amount;
    }

    public void Setting(FishingMethodData_Game_5 fishData, float hpMultiplier = 1f)
    {
        gameObject.SetActive(true);

        maxHp = fishData.maxHp;
        Hp = (int)(fishData.maxHp * hpMultiplier);

        maxSp = fishData.maxSp;
        Sp = maxSp;

        maxCp = fishData.maxCp;
        Cp = fishData.maxCp * 0.2f;

        speed = fishData.speed;

        lowHpCpMultiplier = fishData.lowHpCpMultiplier;

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
        transform.rotation = Quaternion.Euler(0, 0, angle);

        isAlive = true;
        isStunned = false;

        GlobalStateObserver.NotifyFishingStateChanged(true);

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);

        moveCoroutine = StartCoroutine(MoveCoroutine());
        rotateCoroutine = StartCoroutine(RotateCoroutine());
        completeCoroutine = StartCoroutine(CompleteCoroutine());
    }
    public void Setting(FishingMethodData_Game_1 fishData, float hpMultiplier = 1f)
    {
        gameObject.SetActive(true);

        maxHp = fishData.maxHp;
        Hp = (int)(fishData.maxHp * hpMultiplier);

        maxSp = fishData.maxSp;
        Sp = maxSp;

        maxCp = fishData.maxCp;
        Cp = fishData.maxCp * 0.2f;

        speed = fishData.speed;

        lowHpCpMultiplier = fishData.lowHpCpMultiplier;

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
        transform.rotation = Quaternion.Euler(0, 0, angle);

        isAlive = true;
        isStunned = false;

        GlobalStateObserver.NotifyFishingStateChanged(true);

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);

        moveCoroutine = StartCoroutine(MoveCoroutine());
        rotateCoroutine = StartCoroutine(RotateCoroutine());
        completeCoroutine = StartCoroutine(CompleteCoroutine());
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
    IEnumerator CompleteCoroutine()
    {
        while (true)
        {
            yield return null;

            if (isStunned || !isAlive) continue;

            ModifyCp(-maxCp * Time.deltaTime * 0.03f);
        }
    }
    void Rotate()
    {
        // 목표 각도 설정
        targetAngle = Random.Range(0f, 360f);

        // 새 방향 계산
        direction = new Vector2(Mathf.Cos(targetAngle * Mathf.Deg2Rad), Mathf.Sin(targetAngle * Mathf.Deg2Rad)).normalized;
    }
    void Stun()
    {
        isStunned = true;

        if (!isAlive) return;

        CursorController_Game_5.Instance.SetMode(false);

        stunHandle = DOVirtual.Int(0, maxSp, PlayerStat.Stat.stunDuration, value =>
        {
            Sp = value;
        }).OnComplete(() =>
        {
            isStunned = false;

            stunHandle = null;

            CursorController_Game_5.Instance.SetMode(true);
            
         }).SetEase(Ease.Linear);
    }
    void Die()
    {
        isAlive = false;

        if (stunHandle != null && stunHandle.IsActive() && stunHandle.IsPlaying()) stunHandle.Kill();

        Sp = 0;

        Complete();
    }
    public void Cancel()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
        if (completeCoroutine != null) StopCoroutine(completeCoroutine);

        if (stunHandle != null && stunHandle.IsActive() && stunHandle.IsPlaying()) stunHandle.Kill();

        Fishing_Game.fishing_Game.CancelFishing();

        GlobalStateObserver.NotifyFishingStateChanged(false);

        DOVirtual.DelayedCall(0.5f, () => { gameObject.SetActive(false); });
    }
    public void Complete()
    {
        Debug.Log("성공");

        Cancel();

        Fishing_Game.fishing_Game.Complete();
    }
}
