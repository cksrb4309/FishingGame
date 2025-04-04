using UnityEngine;

public class BasicProjectile : Projectile
{
    [SerializeField] ObjectPoolID objectPoolID;

    Rigidbody2D rb = null;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void OnSettingComplete()
    {
        rb.linearVelocity = transform.right * speed;

        Invoke(nameof(DisableProjectile), range / speed);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IPlayerDamagable damagable))
        {
            damagable.ReceiveDamage(1f);
        }
        gameObject.SetActive(false);

        PoolManager.ReturnObj(objectPoolID, (Projectile)this);
    }
    void DisableProjectile()
    {
        gameObject.SetActive(false);
    }
}
