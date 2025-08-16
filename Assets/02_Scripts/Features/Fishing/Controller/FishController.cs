using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; private set; } = null;

    [SerializeField] TMP_Text hpText, spText, cpText;
    [SerializeField] Image hpFillImage, spFillImage, cpFillImage;

    protected bool isAlive, isStunned;

    protected int maxHp, hp, maxSp, sp;

    protected float cp, maxCp, lowHpCpMultiplier = 0f;

    protected Tween stunHandle = null;

    Coroutine completeCoroutine = null;
    public Vector3 WorldPosition => transform.position;
    public Vector2 LocalPosition => transform.localPosition;

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
    public virtual void Cancel()
    {
        if (completeCoroutine != null) StopCoroutine(completeCoroutine);

        Fishing_Game.fishing_Game.CancelFishing();

        DOVirtual.DelayedCall(0.5f, () => { gameObject.SetActive(false); });
    }
    public void Complete()
    {
        Cancel();

        Fishing_Game.fishing_Game.Complete();
    }
    void Stun()
    {
        isStunned = true;

        if (!isAlive) return;

        CursorController.Instance.SetMode(false);

        stunHandle = DOVirtual.Int(0, maxSp, PlayerStat.Stat.stunDuration, value =>
        {
            Sp = value;
        }).OnComplete(() =>
        {
            isStunned = false;

            stunHandle = null;

            CursorController.Instance.SetMode(true);

        }).SetEase(Ease.Linear);
    }
    protected virtual void Die()
    {
        isAlive = false;

        if (stunHandle != null && stunHandle.IsActive() && stunHandle.IsPlaying()) stunHandle.Kill();

        Sp = 0;

        Complete();
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
    public virtual void Setting(FishingMethodData fishData, float hpMultiplier = 1f)
    {
        gameObject.SetActive(true);

        maxHp = fishData.maxHp;
        Hp = (int)(fishData.maxHp * hpMultiplier);

        maxSp = fishData.maxSp;
        Sp = maxSp;

        maxCp = fishData.maxCp;
        Cp = fishData.maxCp * 0.2f;

        lowHpCpMultiplier = fishData.lowHpCpMultiplier;

        isAlive = true;
        isStunned = false;

        if (completeCoroutine != null) StopCoroutine(completeCoroutine);

        completeCoroutine = StartCoroutine(CompleteCoroutine());

        Instance = this;
    }
}
