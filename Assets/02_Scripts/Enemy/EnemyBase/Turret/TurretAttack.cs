using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [Header("투사체 공격 세팅")]
    [SerializeField] ProjectileSet projectileSet;

    [Header("메인 공격 딜레이")]
    [SerializeField] float mainAttackInterval;

    [Header("어그로가 풀리기 위한 위치 차이 값")]
    [SerializeField] float aggroDistance;

    [Header("어그로가 풀리기 위한 시간")]
    [SerializeField] float aggroTime;

    Transform targetTransform = null;

    Coroutine attackCoroutine = null;
    Coroutine aggroCoroutine = null;

    float aggroDuration;

    bool shouldAttackStop = false;

    public void SetTarget()
    {
        if (targetTransform == null) targetTransform = PlayerManager.PlayerTransform;

        if (attackCoroutine == null) StartAttack();

        aggroDuration = aggroTime;

        projectileSet.Enable();

        shouldAttackStop = false;

        if (aggroCoroutine == null) aggroCoroutine = StartCoroutine(AggroCoroutine());
    }
    void StartAttack()
    {
        projectileSet.TargetTransformSetting(targetTransform);
        projectileSet.FireTransformSetting(transform);

        attackCoroutine = StartCoroutine(AttackCoroutine());
    }
    protected virtual IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(mainAttackInterval);

            if (shouldAttackStop)
            {
                shouldAttackStop = false;

                attackCoroutine = null;

                yield break;
            }

            yield return projectileSet.LaunchProjectiles();
        }
    }
    protected virtual IEnumerator AggroCoroutine()
    {
        float aggroDistance = this.aggroDistance * this.aggroDistance;

        while (aggroDuration > 0f)
        {
            yield return new WaitForSeconds(0.49f);

            if (aggroDistance > Vector3.SqrMagnitude(targetTransform.position - transform.position))
            {
                aggroDuration = aggroTime;
            }
            else
            {
                aggroDuration -= 0.49f;
                Debug.Log(aggroDuration);
            }
        }

        ReleaseAggro();
    }
    void ReleaseAggro()
    {
        projectileSet.Disable();

        shouldAttackStop = true;

        aggroCoroutine = null;
    }
    public void Start()
    {
        projectileSet.CreatePoolObject();
    }
    public void OnDestroy()
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);

        if (aggroCoroutine != null) StopCoroutine(aggroCoroutine);
    }
}
