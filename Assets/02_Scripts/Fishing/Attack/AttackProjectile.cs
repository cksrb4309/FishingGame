using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CircleCollider2D circleCollider;
    
    Coroutine returnCoroutine = null;
    bool isAttack = false;
    public void Setting()
    {
        gameObject.SetActive(true);

        float speed = FishingData.MiniGame_3_Data.ProjectileSpeed;

        rb.linearVelocity = transform.right * FishingData.MiniGame_3_Data.ProjectileSpeed;

        transform.localScale = Vector3.one * FishingData.MiniGame_3_Data.ProjectileSize * 2;
    }
    private void OnEnable()
    {
        isAttack = true;

        returnCoroutine = StartCoroutine(ReturnCoroutine());
    }
    private void OnDisable()
    {
        if (returnCoroutine != null) StopCoroutine(returnCoroutine);
    }
    IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSeconds(5f);

        gameObject.SetActive(false);

        PoolManager.ReturnObj(ObjectPoolID.AttackProjectile, this);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttack) return;

        FishController.Instance.Attack(FishingData.MiniGame_3_Data.AttackDamage);

        gameObject.SetActive(false);

        PoolManager.ReturnObj(ObjectPoolID.AttackProjectile, this);
    }
}
