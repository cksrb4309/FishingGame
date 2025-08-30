using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    public static PlayerParry Instance { get; private set; } = null;

    [SerializeField] Transform pivot;

    [SerializeField] ParticleSystem[] parryParticles;

    [SerializeField] float interval;
    [SerializeField] float padding;

    float[] ranges = new float[4];

    /// <summary>
    /// 투사체 패링 실행
    /// </summary>
    private void Parry(InputAction.CallbackContext context) 
    {
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

        // 만약 -1이라면 바깥쪽에 위치한 것임으로 리턴
        if (parryRangeIndex == -1) return;

        // 진행 중이던 투사체 이동 두트윈 취소
        selectProjectile.transform.DOKill();

        // 이펙트 적용과 공격 로직 시퀀스 생성
        Sequence seq = DOTween.Sequence();

        // 이펙트 효과 추가
        seq.Append(ParryEffect(selectProjectile));

        // 공격 로직 추가
        seq.AppendCallback(ParryAttack(selectProjectile, parryRangeIndex));
    }
    private Tween ParryEffect(EnemyProjectile projectile)
    {
        return projectile.transform.DOJump(Enemy.Current.transform.position, Random.Range(-2f, 2f), 1, 0.25f).SetEase(Ease.Linear);
    }
    private TweenCallback ParryAttack(EnemyProjectile projectile, int parryRangeIndex)
    {
        return () => { Enemy.Current.Hit(parryRangeIndex); projectile.EnemyHit(); };
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
    #region Unity Methods
    private void Start()
    {
        for (int i = 0; i < ranges.Length; i++)
            ranges[i] = padding + ((i+1) * interval);
    }
    private void OnEnable()
    {
        InputManager.GetInputAction(InputType.Parry).action.performed += Parry;
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.Parry);
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
