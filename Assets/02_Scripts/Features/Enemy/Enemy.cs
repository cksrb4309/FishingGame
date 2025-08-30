using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Current { get; private set; } = null;

    public EnemyData enemyData;

    Coroutine patternCoroutine = null;

    private void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        Current = this;

        patternCoroutine = StartCoroutine(PatternCoroutine());
    }
    public void Hit(float damage)
    {

    }
    public float GetHealthRatio()
    {
        return 1f;
    }
    private IEnumerator PatternCoroutine()
    {
        enemyData.patternSet.Init();

        yield return new WaitForSeconds(1f);

        while (true)
        {
            EnemyPattern pattern = enemyData.patternSet.GetPattern();

            for (int i = 0; i < pattern.GetProjectileCount(); i++)
            {
                EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

                EnemyProjectile projectile = PoolManager.GetObj(enemyProjectileSet.enemyProjectile);

                projectile.Spawn(enemyProjectileSet.projectileLine.Spline, enemyProjectileSet.moveAnimationCurve);

                if (i < pattern.GetProjectileCount() - 1)
                    yield return enemyProjectileSet.nextAttackDelay;
            }

            yield return new WaitForSeconds(enemyData.attackDelay);
        }
    }
}
