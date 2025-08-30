using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [TabGroup("기본정보")] public string enemyName;
    [TabGroup("기본정보")] public float enemyMaxHp;
    [TabGroup("기본정보")] public Sprite enemySprite;

    [TabGroup("전투")] public EnemyPatternSet patternSet;
    [TabGroup("전투")] public float attackDelay;

    [TabGroup("스폰")] public Vector3 spawnPosition;

}
