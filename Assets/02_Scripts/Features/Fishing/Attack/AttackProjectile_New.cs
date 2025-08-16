using UnityEngine;
using System.Collections;

public class AttackProjectile_New : MonoBehaviour
{
    [SerializeField] ObjectPoolID poolID = ObjectPoolID.AttackProjectile_5_1;

    [SerializeField] float speed;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] CircleCollider2D circleCollider;
    
    Coroutine returnCoroutine = null;

    bool isAttack = false;

    public void Setting()
    {
        gameObject.SetActive(true);

        rb.linearVelocity = transform.right * speed;
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

        PoolManager.ReturnObj(poolID, this);

        gameObject.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttack) return;

        FishController.Instance.ModifyHp(-PlayerStat.Stat.projectileDamage);
        FishController.Instance.ModifySp(-PlayerStat.Stat.stunAccumulation);

        gameObject.SetActive(false);

        PoolManager.ReturnObj(poolID, this);
    }
}
