using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AttackAreaCircle : MonoBehaviour
{
    [SerializeField] SpriteRenderer border;
    [SerializeField] SpriteRenderer fillImage;

    float range = 0f;
    bool isComplete = false;
    public void Setting(Vector2 position)
    {
        isComplete = false;

        transform.position = position;

        range = FishingData.MiniGame_1_Data.AttackRange;

        border.size = Vector2.one * range * 2f;
        fillImage.size = border.size;

        fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, 0f);
        fillImage.DOFade(1f, 0.3f).OnComplete(Attack);
    }

    void Attack()
    {
        float distance = Vector2.Distance(transform.position, FishController_New.Instance.WorldPosition);

        if (distance <= range)
        {
            FishController_New.Instance.ModifyHp(-PlayerStat.Stat.projectileDamage);
            FishController_New.Instance.ModifySp(-PlayerStat.Stat.stunAccumulation);
        }

        border.DOFade(0, 0.5f).OnComplete(Complete);
        fillImage.DOFade(0, 0.5f).OnComplete(Complete);
    }
    void Complete()
    {
        if (isComplete) return;
        isComplete = true;

        PoolManager.ReturnObj(ObjectPoolID.AttackArea, this);

        gameObject.SetActive(false);
        Color c = border.color;
        c.a = 1f;
        border.color = c;
    }
}
