using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Ease ease;

    private Tween moveTween;

    public void Spawn(Spline spline, AnimationCurve animationCurve)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        ProjectileManager.ProjectileRegister(this);

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
        Cancel();

        ReturnPool();
    }
    public void EnemyHit()
    {
        Cancel();

        ReturnPool();
    }
    public void Cancel()
    {
        moveTween?.Kill(); // 이동 취소

        ProjectileManager.ProjectileUnregister(this); // 등록 해제
    }
    private void ReturnPool()
    {
        gameObject.SetActive(false);

        PoolManager.ReturnObj<EnemyProjectile>(this);
    }
}
