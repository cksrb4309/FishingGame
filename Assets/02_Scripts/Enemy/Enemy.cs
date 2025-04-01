using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] EnemyData enemyData;

    public void ReceiveDamage(float damage)
    {
        Debug.Log("ReceiveDamage " + damage.ToString());
    }
    public void OnPlayerDetected(Transform playerTransform)
    {

    }
}
