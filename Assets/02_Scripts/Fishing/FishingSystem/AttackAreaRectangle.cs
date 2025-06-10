using UnityEngine;

public class AttackAreaRectangle : MonoBehaviour
{
    [SerializeField] RectTransform startPosition;
    [SerializeField] RectTransform endPosition;
    [SerializeField] RectTransform fishPosition;

    float attackCooldownTime = 1f;
    float attackInterval = 1;
    float attackRange = 1f;
    int attackDamage = 1;

    bool IsObjectNearInfiniteLine(Vector2 startPos, Vector2 endPos, Vector2 targetPos, float range)
    {
        Vector2 dir = (endPos - startPos).normalized;
        Vector2 toTarget = targetPos - startPos;

        // 투영 거리 계산은 필요 없지만 구해둠
        float projection = Vector2.Dot(toTarget, dir);

        // 직선 상 투영점 좌표 (범위 제한 없이 사용)
        Vector2 projectedPoint = startPos + dir * projection;

        // target에서 직선까지 수직 거리 계산
        float perpendicularDistance = Vector2.Distance(targetPos, projectedPoint);

        // range를 직선과 target 간 최소 거리 제한으로 사용
        return perpendicularDistance <= range;
    }
    private void Update()
    {
        if (attackCooldownTime <= 0f)
        {
            if (IsObjectNearInfiniteLine(startPosition.anchoredPosition, endPosition.anchoredPosition,fishPosition.anchoredPosition, attackRange))
            
                FishController.Instance.Attack(attackDamage);

            attackCooldownTime = attackInterval;
        }
        attackCooldownTime -= Time.deltaTime;
    }

    private void OnEnable()
    {
        attackCooldownTime = FishingData.MiniGame_2_Data.AttackInterval;
        attackInterval = FishingData.MiniGame_2_Data.AttackInterval;
        attackDamage = FishingData.MiniGame_2_Data.AttackDamage;
        attackRange = FishingData.MiniGame_2_Data.AttackRange;
    }
}
