using System.Collections;
using UnityEngine;

public class FishPatternObj : MonoBehaviour
{
    static Transform patternObjParent = null;
    [SerializeField] ObjectPoolID objectPoolID;
    [SerializeField] SpriteRenderer[] fillSpriteRenderers;
    [SerializeField] SpriteRenderer[] borderSpriteRenderers;
    [SerializeField] GameObject colliderGroup;
    [SerializeField] Rigidbody2D rb;
    int damage;
    bool isAttack = false;

    private void Awake()
    {
        if (patternObjParent == null) patternObjParent = GameObject.Find("PatternObjParent").transform;

        transform.SetParent(patternObjParent);
    }
    public void Enable(int damage, float delay)
    {
        gameObject.SetActive(true);

        this.damage = damage;

        StartCoroutine(EnableCoroutine(delay));
    }

    IEnumerator EnableCoroutine(float delay)
    {
        float t = 0;
        float speed = delay > 0f ? 1f / delay : 6000f;

        Color[] fillColors = new Color[fillSpriteRenderers.Length];
        Color[] borderColors = new Color[borderSpriteRenderers.Length];
        for (int i = 0; i < fillSpriteRenderers.Length; i++) fillColors[i] = fillSpriteRenderers[i].color;
        for (int i = 0; i < borderSpriteRenderers.Length; i++)
        {
            borderColors[i] = borderSpriteRenderers[i].color;
            borderColors[i].a = 1f;
            borderSpriteRenderers[i].color = borderColors[i];
        }
        while (t <= 1f)
        {
            for (int i = 0; i < fillSpriteRenderers.Length; i++)
            {
                fillColors[i].a = t;
                fillSpriteRenderers[i].color = fillColors[i];
            }


            t += speed * Time.deltaTime;

            yield return null;
        }

        for (int i = 0; i < fillSpriteRenderers.Length; i++) fillColors[i].a = 1f;
        for (int i = 0; i < fillSpriteRenderers.Length; i++) fillSpriteRenderers[i].color = fillColors[i];

        isAttack = true;

        colliderGroup.SetActive(true);
        rb.WakeUp();

        yield return new WaitForSeconds(0.2f);

        rb.WakeUp();

        colliderGroup.SetActive(false);

        gameObject.SetActive(false);

        if (isAttack)
            FishPatternController.Instance.ReceiveDamage(FishingData.MiniGame_4_Data.AttackDamage);
        
        isAttack = false;

        PoolManager.ReturnObj(objectPoolID, this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttack) return;

        isAttack = false;

        UserMainController.Instance.ReceiveDamage(damage);
    }
}
