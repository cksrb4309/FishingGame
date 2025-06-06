using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AttackArea : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform myRect;
    [SerializeField] Image fillImage;

    float range = 0f;

    public void Setting(Vector2 anchoredPosition)
    {
        myRect.anchoredPosition = anchoredPosition;

        range = FishingData.MiniGame_1_Data.AttackRange;

        myRect.sizeDelta = Vector2.one * range * 2f;

        canvasGroup.alpha = 1f;
        fillImage.fillAmount = 0f;
        fillImage.DOFillAmount(1f, FishingData.MiniGame_1_Data.AttackDelay).OnComplete(Attack);
    }

    void Attack()
    {
        float distance = Vector2.Distance(myRect.anchoredPosition, FishController.Instance.Position);

        if (distance <= range) FishController.Instance.Attack(FishingData.MiniGame_1_Data.AttackDamage);

        canvasGroup.DOFade(0f, 0.5f).OnComplete(Complete);
    }
    void Complete()
    {
        PoolManager.ReturnObj(ObjectPoolID.AttackArea, this);

        gameObject.SetActive(false);
    }
}
