using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] float projectileSpeed = 50f;

    Rigidbody2D rb = null;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Setting(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);

        gameObject.SetActive(true);

        rb.linearVelocity = transform.right * projectileSpeed;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IPlayerDamagable damagable))
        {
            damagable.ReceiveDamage(1f);
        }

        gameObject.SetActive(false);

        PoolManager.ReturnObj(ObjectPoolID.PlayerBullet_1, this);
    }
}
