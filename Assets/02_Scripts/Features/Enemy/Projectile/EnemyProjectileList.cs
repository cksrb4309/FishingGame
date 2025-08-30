using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[Serializable]
public class EnemyProjectileList
{
    public List<EnemyProjectileSet> enemyProjectileSets;
}

[Serializable]
public class EnemyProjectileSet
{
    public EnemyProjectile enemyProjectile;     // 투사체 Prefab
    public SplineContainer projectileLine;      // 투사체 궤적
    public AnimationCurve moveAnimationCurve;   // 투사체 
    public float nextAttackDelay;               // 다음 공격 지연
}
