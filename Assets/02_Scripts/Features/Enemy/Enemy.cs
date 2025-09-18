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
        // 적 패턴 초기화
        enemyData.patternSet.Init();

        // 몹 등장 후 기본 1초 대기
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // 적의 패턴을 하나 가져옴
            EnemyPattern pattern = enemyData.patternSet.GetPattern();

            // 1차 전조 이펙트 재생
            PreEffect(pattern);

            // 기본 1초의 2차 전조 이펙트 대기시간
            yield return new WaitForSeconds(1f);

            // 2차 전조 이펙트 재생
            yield return PreAttackEffectCoroutine(pattern);

            // 전조 이펙트 나오고 나서 등장하는 공격 딜레이 적용
            yield return new WaitForSeconds(pattern.preAttackDelay);

            // 공격 적용
            yield return AttackCoroutine(pattern);

            // 몹마다 존재하는 (다음 패턴까지의)공격 딜레이 적용
            yield return new WaitForSeconds(enemyData.attackDelay);
        }
    }
    private void PreEffect(EnemyPattern pattern)
    {
        if (pattern.preEffect != null)
        {
            ParticleSystem effect = PoolManager.GetObj(pattern.preEffect);

            if (!effect.gameObject.activeSelf) effect.gameObject.SetActive(true);

            effect.transform.position = transform.position;

            DOVirtual.DelayedCall(2f, () => { effect.gameObject.SetActive(false); PoolManager.ReturnObj(effect); });
        }
    }
    private IEnumerator AttackCoroutine(EnemyPattern pattern)
    {
        for (int i = 0; i < pattern.GetProjectileCount(); i++)
        {
            EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

            EnemyProjectile projectile = PoolManager.GetObj(enemyProjectileSet.enemyProjectile);

            projectile.transform.position = Vector3.one * 100f;

            projectile.Spawn(enemyProjectileSet.projectileLine.Spline, enemyProjectileSet.moveAnimationCurve);

            if (i < pattern.GetProjectileCount() - 1)
                yield return new WaitForSeconds(enemyProjectileSet.nextAttackDelay);
        }
    }
    private IEnumerator PreAttackEffectCoroutine(EnemyPattern pattern)
    {
        List<PreEffectTuple> list = new List<PreEffectTuple>();

        float value = 0f;
        float minDelay = float.MaxValue; 

        for (int i = 0; i < pattern.GetProjectileCount(); i++)
        {
            EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

            float delay = value + enemyProjectileSet.enemyProjectile.GetDuration();

            list.Add(new PreEffectTuple(
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


public class PreEffectTuple
{
    public ParticleSystem effectParticle;
    public AudioClip effectAudio;
    public float delay;

    public PreEffectTuple(ParticleSystem effectParticle, AudioClip effectAudio, float delay)
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