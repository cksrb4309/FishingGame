using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] TurretBlock turret;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        enemy?.OnPlayerDetected(collision.transform);
        turret?.OnPlayerDetected();
    }
}
