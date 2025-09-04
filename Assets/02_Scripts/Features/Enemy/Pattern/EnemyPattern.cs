using System;
using UnityEngine;

[Serializable]
public class EnemyPattern
{
    public ParticleSystem preEffect = null; // 전조 이펙트
    public float preAttackDelay = 0f; // 전조 후 공격 딜레이
    public EnemyProjectileList enemyProjectileList; // 투사체 요소
    public PatternCodition patternCondition; // 패턴이 나오기 위한 조건

    public bool IsMet() => patternCondition == null ? true : patternCondition.IsMet();
    public int GetProjectileCount() => enemyProjectileList.enemyProjectileSets.Count;
    public EnemyProjectileSet SelectProjectileSet(int index) => enemyProjectileList.enemyProjectileSets[index];
}
