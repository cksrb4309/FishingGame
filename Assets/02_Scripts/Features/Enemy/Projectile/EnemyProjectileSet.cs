using System;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class EnemyProjectileSet
{
    public EnemyProjectileTemplate projectileTemplate;

    public SplineContainer projectileLine;      // 투사체 궤적
    public AnimationCurve moveAnimationCurve;   // 투사체 이동 커브

    public float nextAttackDelay;               // 다음 공격 지연
}
