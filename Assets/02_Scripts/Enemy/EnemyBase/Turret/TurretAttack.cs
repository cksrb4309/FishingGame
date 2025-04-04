using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    #region 이전버전
    //[Header("공격 간격")]
    //[SerializeField] float attackInterval;

    //[Header("투사체 데미지")]
    //[SerializeField] List<float> attackDamages;

    //[Header("투사체 스피드")]
    //[SerializeField] float[] projectileSpeeds;

    //[Header("투사체 간격")]
    //[SerializeField] List<float> projectileIntervals;

    //[Header("투사체 발사각")]
    //[SerializeField] List<float> shotAngles;

    //[Header("공격 투사체")]
    //[SerializeField] List<ObjectPoolID> projectiles;

    //[Header("투사체 발사 개수")]
    //[SerializeField] int[] projectileCount;

    //[Header("투사체 간의 퍼지는 각도")]
    //[SerializeField] float[] spreadAngles;

    //[Header("투사체의 사정거리")]
    //[SerializeField] List<float> projectileMaxRanges;


    #endregion

    [Header("투사체 공격 세팅")]
    [SerializeField] ProjectileSet 

    [Header("어그로가 풀리기 위한 위치 차이 값")]
    [SerializeField] float aggroDistance;

    [Header("어그로가 풀리기 위한 시간")]
    [SerializeField] float aggroTime;

    Transform targetTransform = null;

    Coroutine attackCoroutine = null;
    Coroutine aggroCoroutine = null;

    float aggroDuration;

    bool shouldStopAttack = false;

    public void SetTarget()
    {
        if (targetTransform == null) targetTransform = PlayerManager.PlayerTransform;

        if (attackCoroutine == null) attackCoroutine = StartCoroutine(AttackCoroutine());

        aggroDuration = aggroTime;

        shouldStopAttack = false;

        if (aggroCoroutine == null) aggroCoroutine = StartCoroutine(AggroCoroutine());
    }
    protected virtual IEnumerator AttackCoroutine()
    {
        #region 이전 버전
        //WaitForSeconds attackInterval = new WaitForSeconds(this.attackInterval);
        //WaitForSeconds[] projectileIntervals = new WaitForSeconds[this.projectileIntervals.Count];

        //for (int i = 0; i < projectileIntervals.Length; i++) projectileIntervals[i] = new WaitForSeconds(this.projectileIntervals[i]);

        //while (true)
        //{
        //    yield return attackInterval; // 공격 딜레이 적용

        //    if (shouldStopAttack)
        //    {
        //        shouldStopAttack = false;

        //        attackCoroutine = null; yield break;
        //    }

        //    for (int index = 0; index < projectiles.Count; index++) // 투사체 수만큼 반복
        //    {
        //        Vector2 dir = (targetTransform.position - transform.position).normalized;

        //        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //        for (int count = 0; count < projectileCount[index]; count++)
        //        {
        //            float projectileAngle = spreadAngles[index] * (count - ((projectileCount[index] - 1) * 0.5f));

        //            Projectile projectile = PoolManager.GetObj<Projectile>(ObjectPoolID.TurretProjectile_1);

        //            projectile.transform.position = transform.position;

        //            projectile.Setting(attackDamages[index], projectileMaxRanges[index], projectileSpeeds[index], targetAngle + projectileAngle, targetTransform.position);
        //        }

        //        yield return projectileIntervals[index];
        //    }
        //}
        #endregion


    }
    protected virtual IEnumerator AggroCoroutine()
    {
        float aggroDistance = this.aggroDistance * this.aggroDistance;

        while (aggroDuration > 0f)
        {
            yield return new WaitForSeconds(0.49f);

            if (aggroDistance > Vector3.SqrMagnitude(targetTransform.position - transform.position))
            {
                aggroDuration = aggroTime;
            }
            else
            {
                aggroDuration -= 0.49f;
            }
        }

        ReleaseAggro();
    }
    void ReleaseAggro()
    {
        shouldStopAttack = true;

        aggroCoroutine = null;
    }

    public void Start()
    {
        foreach (ObjectPoolID objectPoolID in projectiles) PoolManager.CreatePool<Projectile>(objectPoolID);
    }
}
