using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour
{
    public static Enemy Current { get; private set; } = null;

    public EnemyData enemyData;

    public AudioSource audioSource;

    public ParticleSystem enemyHitParticle;

    public float testWarningDelay = 1f;

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
        if (pattern.patternPreParticle != null)
        {
            PoolManager.ParticlePlay(pattern.patternPreParticle, transform.position);
        }
    }
    private IEnumerator AttackCoroutine(EnemyPattern pattern)
    {
        // 지연 적용시킬 투사체 발사 부분
        void Attack(EnemyProjectileSet enemyProjectileSet)
        {
            EnemyProjectile projectile = PoolManager.GetObj(enemyProjectileSet.projectileTemplate.enemyProjectile, true);

            projectile.transform.position = Vector3.one * 100f;

            projectile.Spawn(enemyProjectileSet.projectileLine.Spline, enemyProjectileSet.moveAnimationCurve);
            SplineUtility.EvaluatePosition(enemyProjectileSet.projectileLine.Spline, 0f);

            // 발사 이펙트 재생
            PoolManager.ParticlePlay(enemyProjectileSet.projectileTemplate.shotParticle, enemyProjectileSet.projectileLine.Spline);
        }

        for (int i = 0; i < pattern.GetProjectileCount(); i++)
        {
            EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

            // 사전 경고 이펙트 재생
            PoolManager.ParticlePlay(enemyProjectileSet.projectileTemplate.warningParticle, enemyProjectileSet.projectileLine.Spline);

            // 이펙트 재생 후 testWarningDelay만큼 지연 후 투사체 발사
            DOVirtual.DelayedCall(testWarningDelay, () => Attack(enemyProjectileSet));

            // EnemyProjectileSet의 다음 딜레이 적용
            if (i < pattern.GetProjectileCount() - 1)
                yield return new WaitForSeconds(enemyProjectileSet.nextAttackDelay);
        }
    }
    private IEnumerator PreAttackEffectCoroutine(EnemyPattern pattern)
    {
        List<PreEffectTuple> preAttackEffects = new List<PreEffectTuple>();

        float value = 0f;
        float minDelay = float.MaxValue; 

        for (int i = 0; i < pattern.GetProjectileCount(); i++)
        {
            EnemyProjectileSet enemyProjectileSet = pattern.SelectProjectileSet(i);

            float delay = value + enemyProjectileSet.projectileTemplate.enemyProjectile.GetDuration();

            preAttackEffects.Add(new PreEffectTuple(
                enemyProjectileSet.projectileTemplate.preParticles.Count > i ?
                enemyProjectileSet.projectileTemplate.preParticles[i] :
                enemyProjectileSet.projectileTemplate.preParticles[0],
                enemyProjectileSet.projectileTemplate.preAudio,
                delay
            ));

            if (delay < minDelay) minDelay = delay; 

            value += enemyProjectileSet.nextAttackDelay;
        }

        if (minDelay > 0f) foreach (var item in preAttackEffects) item.DecreaseDelay(minDelay);
            
        // ✅ delay 기준으로 오름차순 정렬
        preAttackEffects.Sort((a, b) => a.delay.CompareTo(b.delay));


        // ✅ 순차 실행
        for (int i = 0; i < preAttackEffects.Count; i++)
        {
            ParticleSystem effectParticle = PoolManager.GetObj(preAttackEffects[i].effectParticle);

            if (!effectParticle.gameObject.activeSelf)  effectParticle.gameObject.SetActive(true);

            effectParticle.Play();

            audioSource.PlayOneShot(preAttackEffects[i].effectAudio);

            if (i < preAttackEffects.Count - 1)
                yield return new WaitForSeconds(preAttackEffects[i + 1].delay - preAttackEffects[i].delay);
        }
    }
    public void PlayHitParticle()
    {
        PoolManager.ParticlePlay(enemyHitParticle, transform.position);
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