using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPatternObj : MonoBehaviour
{
    [SerializeField] ObjectPoolID objectPoolID;
    [SerializeField] SpriteRenderer[] fillSpriteRenderers;

    GameObject colliderGroup;
    int damage;

    private void Awake()
    {
        colliderGroup = transform.Find("ColliderGroup").gameObject;
    }
    public void Enable(int damage, float delay)
    {
        this.damage = damage;

        StartCoroutine(EnableCoroutine(delay));
    }

    IEnumerator EnableCoroutine(float delay)
    {
        float t = 0;
        float speed = delay > 0f ? 1f / delay : 6000f;
        Color color = new Color(1, 1, 1, 0);

        while (t <= 1f)
        {
            color.a = t;

            for (int i = 0; i < fillSpriteRenderers.Length; i++)
                fillSpriteRenderers[i].color = color;
            

            t += speed * Time.deltaTime;

            yield return null;
        }
        color.a = 1;

        for (int i = 0; i < fillSpriteRenderers.Length; i++)
            fillSpriteRenderers[i].color = color;

        colliderGroup.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        colliderGroup.SetActive(false);
        
        gameObject.SetActive(false);

        isAttack = false;

        PoolManager.ReturnObj(objectPoolID, this);
    }
    bool isAttack = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttack) return;
        isAttack = true;
        UserController.Instance.ReceiveDamage(damage);
    }
}
