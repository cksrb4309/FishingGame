using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Data/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [Header("공격 투사체")]
    public ObjectPoolID Projectiles;

    [Header("투사체 데미지")]
    public float AttackDamage;

    [Header("투사체 스피드")]
    public float ProjectileSpeed;

    [Header("투사체 발사 개수")]
    public int ProjectileCount;

    [Header("다음 투사체까지의 딜레이")]
    public float ProjectileInterval;

    [Header("투사체 간의 퍼지는 각도")]
    public float SpreadAngles;

    [Header("투사체의 사정거리")]
    public float ProjectileMaxRange;
}
