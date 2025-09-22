using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PlayerParry : MonoBehaviour
{
    public static PlayerParry Instance { get; private set; } = null;

    [Inject] PlayerAnimator playerAnimator;
    [Inject] PlayerEffect playerEffect;

    #region 패링--------------------------------------------------------------------------------

    [SerializeField, TabGroup("패링")] Transform pivot;

    [SerializeField, TabGroup("패링")] float parryUseSp = 20f;
    [SerializeField, TabGroup("패링")] float interval;
    [SerializeField, TabGroup("패링")] float padding;

    [SerializeField, TabGroup("패링")] ParticleSystem[] parryParticles;

    float[] ranges = new float[4];

    /// <summary>
    /// 투사체 패링 실행
    /// </summary>
    private void Parry(InputAction.CallbackContext context)
    {
        if (PlayerData.Data.GetSp() < parryUseSp) return;

        PlayerData.Data.ModifySp(-parryUseSp);

        playerAnimator.Parry();

        // 투사체 매니저에서 패링 가능한 투사체 가져오기
        List<EnemyProjectile> list = ProjectileManager.GetProjectiles(); 

        // 투사체가 없다면 종료
        if (list.Count == 0) return; 

        // 가장 가까이에 있는 탄환 선택
        EnemyProjectile selectProjectile = list
            .OrderBy(p => Vector2.Distance((Vector2)p.transform.position, (Vector2)pivot.transform.position))
            .FirstOrDefault(); 

        // 가까이 있는 탄환이 어느 범위에 있는 지 확인
        int parryRangeIndex = GetParryRangeIndex(Vector3.Distance(selectProjectile.transform.position, pivot.transform.position));

        // 만약 -1이라면 바깥쪽에 위치한 것임으로 리턴과 동시 실패로 판정
        if (parryRangeIndex == -1)
        {
            PlayerParryCombo.Instance?.DecreaseCombo();

            return;
        }
        
        if (parryRangeIndex >= 3)
            PlayerData.Data.ModifySp(parryUseSp * 0.5f);

        PlayerData.Data.ModifySp(parryUseSp * 0.5f);

        PlayerParryCombo.Instance?.IncreaseCombo();

        // 투사체 공격 취소
        selectProjectile.Cancel();

        ParryEffectApply(selectProjectile, parryRangeIndex);
    }
    public void ParryEffectApply(EnemyProjectile projectile, int parryRangeIndex = 4)
    {
        // 투사체 패링 이펙트
        playerEffect.ProjectileHit(projectile.transform.position);

        // 이펙트 적용과 공격 로직 시퀀스 생성
        Sequence seq = DOTween.Sequence();

        // 이펙트 효과 추가
        seq.Append(ParryEffect(projectile));

        // 공격 로직 추가
        seq.AppendCallback(ParryAttack(projectile, parryRangeIndex));
    }
    private Tween ParryEffect(EnemyProjectile projectile)
    {
        return projectile.transform.DOJump(Enemy.Current.transform.position, Random.Range(-2f, 2f), 1, 0.25f).SetEase(Ease.Linear);
    }
    private TweenCallback ParryAttack(EnemyProjectile projectile, int parryRangeIndex)
    {
        return () => projectile.EnemyHit(parryRangeIndex);
    }
    private int GetParryRangeIndex(float distance)
    {
        if (ranges[ranges.Length - 1] < distance) return -1;

        for (int i = 0; i < ranges.Length; i++)
        {
            if (ranges[i] >= distance)
            {
                return ranges.Length - i;
            }
        }

        return -1;
    }
    #endregion

    #region 간파--------------------------------------------------------------------------------

    [SerializeField, TabGroup("간파")] float insightUseSp = 20f;
    [SerializeField, TabGroup("간파")] float insightCooltime = 1f;
    [SerializeField, TabGroup("간파")] float insightDuration = 0.5f;

    [SerializeField, TabGroup("간파")] GameObject insightObj;

    bool canInsight = true;

    private void Insight(InputAction.CallbackContext context)
    {
        if (!canInsight) return;

        if (PlayerData.Data.GetSp() < insightUseSp) return;

        PlayerData.Data.ModifySp(-insightUseSp);

        canInsight = false;
        insightObj.SetActive(true);

        PlayerData.Data.EnableInsight();

        // 간파 상태 종료 함수 간파 지속시간 후 실행
        DOVirtual.DelayedCall(insightDuration, () => { PlayerData.Data.DisableInsight(); insightObj.SetActive(false); });

        // 간파사용 쿨타임 적용
        DOVirtual.DelayedCall(insightCooltime, () => canInsight = true);
    }
    public void InsightComplete()
    {
        PlayerData.Data.ModifySp(insightUseSp);

        PlayerParryCombo.Instance?.IncreaseCombo(2);
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < ranges.Length; i++)
            ranges[i] = padding + ((i+1) * interval);
    }
    private void OnEnable()
    {
        InputManager.GetInputAction(InputType.Parry).action.performed += Parry;
        InputManager.GetInputAction(InputType.Insight).action.performed += Insight;
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.Parry);
        InputManager.Release(InputType.Insight);
    }
    private void OnValidate()
    {
        for (int i = 0; i < ranges.Length; i++)
            ranges[i] = padding + ((i + 1) * interval);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < ranges.Length; i++)
        {
            Gizmos.DrawWireSphere(pivot.position, ranges[i]);
        }
    }
    #endregion
}
