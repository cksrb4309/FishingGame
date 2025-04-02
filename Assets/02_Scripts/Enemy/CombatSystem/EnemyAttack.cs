using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] Transform fireTransform;

    Transform targetTransform = null;

    Coroutine attackCoroutine = null;

    float attackInterval = 0f;

    public void SetTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;

        if (attackCoroutine != null) StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(AttackCoroutine());
    }
    public void Initialize(EnemyData enemyData)
    {
        attackInterval = enemyData.AttackInterval;
    }
    IEnumerator AttackCoroutine()
    {
        if (targetTransform == null)
        {
            Debug.LogError("Å¸°Ù Null");

            yield break;
        }

        WaitForSeconds wait = new WaitForSeconds(attackInterval);

        while (true)
        {
            yield return wait;

            Attack();
        }
    }
    public void Attack()
    {
        EnemyProjectile enemyProjectile = PoolManager.GetObj<EnemyProjectile>(ObjectPoolID.EnemyBullet_1);

        enemyProjectile.transform.position = fireTransform.position;

        Vector3 dir = (targetTransform.position - fireTransform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        enemyProjectile.Setting(angle);
    }
    private void Awake()
    {
        PoolManager.CreatePool<EnemyProjectile>(ObjectPoolID.EnemyBullet_1);
    }
}
