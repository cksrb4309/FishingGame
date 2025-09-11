using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] bool isParryable = true;

    [SerializeField] float duration;
    [SerializeField] float baseDamage;

    private Tween moveTween;

    public void Spawn(Spline spline, AnimationCurve animationCurve)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        if (isParryable) ProjectileManager.ProjectileRegister(this);

        moveTween?.Kill(); // 이전 트윈이 살아있다면 Kill

        float t = 0f;

        moveTween = DOTween.To(
            () => t,
            x =>
            {
                t = x;

                Vector3 pos = SplineUtility.EvaluatePosition(spline, t);
                transform.position = pos;
            },
            1f, duration)
            .SetEase(animationCurve)
            .OnComplete(PlayerHit);
    }
    private void PlayerHit()
    {
        if (!isParryable && PlayerData.Data.IsInSight())
        {
            moveTween?.Kill();

            PlayerParry.Instance.InsightComplete();

            PlayerParry.Instance.ParryEffectApply(this);

            return;
        }

        PlayerData.Data.ModifyHp(-baseDamage);

        PlayerParryCombo.Instance?.DecreaseCombo();

        Cancel();

        ReturnPool();
    }
    public void EnemyHit(int parryRangeIndex)
    {
        EnemyData.Current.ModifyHp(baseDamage * parryRangeIndex * -1f);

        Cancel();

        ReturnPool();
    }
    public void Cancel()
    {
        moveTween?.Kill(); // 이동 취소

        if (isParryable) ProjectileManager.ProjectileUnregister(this); // 등록 해제
    }
    private void ReturnPool()
    {
        gameObject.SetActive(false);

        PoolManager.ReturnObj<EnemyProjectile>(this);
    }
}
