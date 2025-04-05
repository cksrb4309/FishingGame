using UnityEngine;

public class ExplosionBlock : Block
{
    [Header("���� ������")]
    [SerializeField] float explosionDamage;

    [Header("���� ����")]
    [SerializeField] float explosionRange;

    [Header("���� ���� ���̾�")]
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
