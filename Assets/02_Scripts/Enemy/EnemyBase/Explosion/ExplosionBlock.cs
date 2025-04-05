using UnityEngine;

public class ExplosionBlock : Block
{
    [Header("폭발 데미지")]
    [SerializeField] float explosionDamage;

    [Header("폭발 범위")]
    [SerializeField] float explosionRange;

    [Header("폭발 적용 레이어")]
    [SerializeField] LayerMask layerMask;

    protected override void Break()
    {
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(transform.position, explosionRange, layerMask);

        foreach (Collider2D hit in explosionHits)
        {
            hit.GetComponent<IDamagable>().ReceiveDamage(explosionDamage);
        }

        gameObject.SetActive(false);
    }
}
