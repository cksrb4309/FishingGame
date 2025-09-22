using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyProjectileTemplate", menuName = "Enemy/Pattern/EnemyProjectileTemplate")]
public class EnemyProjectileTemplate : ScriptableObject
{
    public EnemyProjectile enemyProjectile;     // 투사체 Prefab
    public AudioClip preAudio;                  // 미리보기 효과음
    public List<ParticleSystem> preParticles;    // 미리보기 효과 이펙트

    public ParticleSystem warningParticle;      // 전조 이펙트 (총알 나오는 자리에 생기는 이펙트)
    public ParticleSystem shotParticle;         // 발사 이펙트 (총알이 꺼내질 때 발생하는 이펙트)
    public ParticleSystem hitParticle;          // 패링 이펙트 (총알이 튕겨질 때 발생하는 이펙트)
}
