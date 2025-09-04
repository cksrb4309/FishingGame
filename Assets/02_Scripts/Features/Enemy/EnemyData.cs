using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public static EnemyData Current { get; private set; }

    [TabGroup("기본정보")] public string enemyName;
    [TabGroup("기본정보")] public float enemyMaxHp;
    [TabGroup("기본정보")] public Sprite enemySprite;

    [TabGroup("전투")] public EnemyPatternSet patternSet;
    [TabGroup("전투")] public float attackDelay;

    [TabGroup("스폰")] public Vector3 spawnPosition;

    public event Action hpModifyAction = null;
    public event Action dieAction = null;

    [SerializeField] float maxHp, hp;

    bool isAlive = true;
    public void Init()
    {
        maxHp = enemyMaxHp;
        hp = enemyMaxHp;
        isAlive = true;

        Current = this;
    }
    public float GetHp() => hp;
    public float GetMaxHp() => maxHp;
    public float GetHpRatio() => hp > 0f ? hp / maxHp : 0f;
    public void ModifyHp(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0f, maxHp);

        hpModifyAction?.Invoke();

        // 몹이 죽었을 경우
        if (hp <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        isAlive = false;

        dieAction?.Invoke();
    }
}
