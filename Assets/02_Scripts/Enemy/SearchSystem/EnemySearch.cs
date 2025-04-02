using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    [SerializeField] Enemy enemy = null;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        enemy?.OnPlayerDetected(collision.transform);
    }
}
