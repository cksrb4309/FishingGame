using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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
        List<TestA> list = new List<TestA>();

        float value = 0f;
        float minDelay = float.MaxValue; 

        for (int i = 0; i < pattern.GetProjectileCount(); i++)
        {
            EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

            float delay = value + enemyProjectileSet.enemyProjectile.GetDuration();

            list.Add(new TestA(
                enemyProjectileSet.effectParticle,
                enemyProjectileSet.effectAudio,
                delay
            ));

            if (delay < minDelay) minDelay = delay; 

            value += enemyProjectileSet.nextAttackDelay;
        }

        if (minDelay > 0f) foreach (var item in list) item.DecreaseDelay(minDelay);
            
        // ✅ delay 기준으로 오름차순 정렬
        list.Sort((a, b) => a.delay.CompareTo(b.delay));


        // ✅ 순차 실행
        for (int i = 0; i < list.Count; i++)
        {
            ParticleSystem effectParticle = PoolManager.GetObj(list[i].effectParticle);

            if (!effectParticle.gameObject.activeSelf)  effectParticle.gameObject.SetActive(true);

            effectParticle.Play();

            audioSource.PlayOneShot(list[i].effectAudio);

            if (i < list.Count - 1)
                yield return new WaitForSeconds(list[i + 1].delay - list[i].delay);
        }
    }
}


public class TestA
{
    public ParticleSystem effectParticle;
    public AudioClip effectAudio;
    public float delay;

    public TestA(ParticleSystem effectParticle, AudioClip effectAudio, float delay)
    {
        this.effectParticle = effectParticle;
        this.effectAudio = effectAudio;
        this.delay = delay;
    }
    public void DecreaseDelay(float amount)
    {
        delay -= amount;
    }
}