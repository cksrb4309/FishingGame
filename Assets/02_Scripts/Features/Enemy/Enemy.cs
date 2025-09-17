using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Current { get; private set; } = null;

    public EnemyData enemyData;

    public AudioSource audioSource;

    Coroutine patternCoroutine = null;

    private void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        Current = this;

        enemyData.Init();

        EnemyUI.Instance.Init();

        patternCoroutine = StartCoroutine(PatternCoroutine());
    }
    private IEnumerator PatternCoroutine()
    {
        enemyData.patternSet.Init();

        yield return new WaitForSeconds(1f);

        while (true)
        {
            EnemyPattern pattern = enemyData.patternSet.GetPattern();

            if (pattern.preEffect != null)
            {
                ParticleSystem effect = PoolManager.GetObj(pattern.preEffect);

                if (!effect.gameObject.activeSelf) effect.gameObject.SetActive(true);

                effect.transform.position = transform.position;

                DOVirtual.DelayedCall(2f, () => { effect.gameObject.SetActive(false); PoolManager.ReturnObj(effect); });
            }

            yield return new WaitForSeconds(1f);

            yield return PreAttackEffect(pattern);

            yield return new WaitForSeconds(pattern.preAttackDelay);

            for (int i = 0; i < pattern.GetProjectileCount(); i++)
            {
                EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

                EnemyProjectile projectile = PoolManager.GetObj(enemyProjectileSet.enemyProjectile);

                projectile.transform.position = Vector3.one * 100f;

                projectile.Spawn(enemyProjectileSet.projectileLine.Spline, enemyProjectileSet.moveAnimationCurve);

                if (i < pattern.GetProjectileCount() - 1)
                    yield return new WaitForSeconds(enemyProjectileSet.nextAttackDelay);
            }

            yield return new WaitForSeconds(enemyData.attackDelay);
        }
    }

    private IEnumerator PreAttackEffect(EnemyPattern pattern)
    {
        for (int i = 0; i < pattern.GetProjectileCount(); i++)
        {
            EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

            ParticleSystem effectParticle = PoolManager.GetObj(enemyProjectileSet.effectParticle);

            if (!effectParticle.gameObject.activeSelf) effectParticle.gameObject.SetActive(true);

            effectParticle.Play();

            audioSource.PlayOneShot(enemyProjectileSet.effectAudio);

            if (i < pattern.GetProjectileCount() - 1) yield return new WaitForSeconds(enemyProjectileSet.nextAttackDelay);
        }
    }
}
