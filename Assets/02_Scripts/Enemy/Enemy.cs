using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerDamagable
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] EnemyAttack enemyAttack;

    public void ReceiveDamage(float damage)
    {
        Debug.Log("ReceiveDamage " + damage.ToString());
    }
    public void OnPlayerDetected(Transform playerTransform)
    {
        enemyAttack.SetTarget(playerTransform);
    }
    private void Awake()
    {
        enemyAttack.Initialize(enemyData);
    }
}
