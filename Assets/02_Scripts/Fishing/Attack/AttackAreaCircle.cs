using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AttackAreaCircle : MonoBehaviour
{
    [SerializeField] SpriteRenderer border;
    [SerializeField] SpriteRenderer fillImage;

    float range = 0f;
    
    public void Setting(Vector2 position)
    {
        transform.position = position;

        range = FishingData.MiniGame_1_Data.AttackRange;

        border.size = Vector2.one * range * 2f;
        fillImage.size = border.size;

        fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 0f);
        fillImage.DOFade(1f, FishingData.MiniGame_1_Data.AttackDelay).OnComplete(Attack);
    }

    void Attack()
    {
        float distance = Vector2.Distance(transform.position, FishController.Instance.WorldPosition);

        if (distance <= range) FishController.Instance.Attack(FishingData.MiniGame_1_Data.AttackDamage);

        border.DOFade(0, 0.5f);
        fillImage.DOFade(0, 0.5f).OnComplete(Complete);
    }
    void Complete()
    {
        PoolManager.ReturnObj(ObjectPoolID.AttackArea, this);

        gameObject.SetActive(false);
        Color c = border.color;
        c.a = 1f;
        border.color = c;
    }
}
