using System;
using UnityEngine;
using DG.Tweening;

public class PlayerData
{
    private static PlayerData data = null;
    public static PlayerData Data
    {
        get
        {
            if (data != null)
            {
                return data;
            }

            data = Init();

            return data;
        }
    }

    bool isInsight = false;

    float maxHp, hp;
    float maxSp, sp;
    float recoveryPerSecond = 20f;

    public event Action hpModifyAction = null;
    public event Action spModifyAction = null;

    Tween spTween;

    public static PlayerData Init()
    {
        return new PlayerData() { maxHp = 100f, hp = 100f, maxSp = 100f, sp = 100f };
    }
    public void ModifyHp(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0f, maxHp);

        hpModifyAction?.Invoke();
    }
    public void ModifySp(float amount)
    {
        sp = Mathf.Clamp(sp + amount, 0f, maxSp);
        spModifyAction?.Invoke();

        // 소비 or 회복 모두 자동 회복 Tween 갱신
        if (sp < maxSp)
        {
            spTween?.Kill();

            float needRecover = maxSp - sp; // 회복해야 할 양
            if (needRecover > 0f)
            {
                float duration = needRecover / recoveryPerSecond;

                spTween = DOTween.To(
                    () => sp,
                    x => { sp = x; spModifyAction?.Invoke(); },
                    maxSp,
                    duration
                ).SetEase(Ease.Linear);
            }
        }
        else
        {
            // 이미 최대치면 트윈 종료
            spTween?.Kill();
        }
    }
    public float GetHp() => hp;
    public float GetSp() => sp;
    public float GetMaxHp() => maxHp;
    public float GetMaxSp() => maxSp;
    public float GetHpRatio() => hp > 0f ? hp / maxHp : 0f;
    public float GetSpRatio() => sp > 0f ? sp / maxSp : 0f;
    public void EnableInsight() => isInsight = true;
    public void DisableInsight() => isInsight = false;
    public bool IsInSight() => isInsight;
}
