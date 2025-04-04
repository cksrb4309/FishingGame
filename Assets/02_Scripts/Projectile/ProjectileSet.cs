using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSet
{
    [Header("사용할 투사체 데이터 셋")]
    [SerializeField] ProjectileData[] projectileDatas;

    [Header("투사체 셋 연속 사용 횟수")]
    [SerializeField] int[] burstCounts;

    [Header("다음 투사체 셋 사용까지의 딜레이")]
    [SerializeField] float[] burstIntervals;

    Transform targetTransform = null;
    Transform fireTransform = null;

    bool shouldStopAttack = false;

    public void FireTransformSetting(Transform fireTransform) => this.fireTransform = fireTransform;
    public void TargetTransformSetting(Transform targetTransform) => this.targetTransform = targetTransform;
    public void Disable() => shouldStopAttack = true;
    public void Enable() => shouldStopAttack = false;
    public IEnumerator LaunchProjectiles()
    {
        for (int index = 0; index < projectileDatas.Length; index++)
        {
            if (shouldStopAttack)
            {
                shouldStopAttack = false; yield break;
            }

            for (int burstCount = 0; burstCount < burstCounts[index]; burstCount++)
            {
                Vector2 dir = (targetTransform.position - fireTransform.position).normalized;
                float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                for (int count = 0; count < projectileDatas[index].ProjectileCount; count++)
                {
                    float projectileAngle = projectileDatas[index].SpreadAngles * (count - ((projectileDatas[index].ProjectileCount - 1) * 0.5f));

                    Projectile projectile = PoolManager.GetObj<Projectile>(ObjectPoolID.TurretProjectile_1);

                    projectile.transform.position = fireTransform.position;

                    projectile.Setting(projectileDatas[index].AttackDamage,
                        projectileDatas[index].ProjectileMaxRange,
                        projectileDatas[index].ProjectileSpeed,
                        targetAngle + projectileAngle,
                        targetTransform.position);

                    yield return projectileDatas[index].ProjectileInterval;
                }
                yield return new WaitForSeconds(burstIntervals[index]);
            }
        }
    }
}
