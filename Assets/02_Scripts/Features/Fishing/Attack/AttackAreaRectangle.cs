using UnityEngine;

public class AttackAreaRectangle : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] Transform endPosition;
    [SerializeField] Transform fishPosition;

    SpriteRenderer spriteRenderer;

    const float SizeY = 60f;

    float attackCooldownTime = 1f;
    float attackInterval = 1;
    float attackRange = 1f;
    int attackDamage = 1;
    private void Awake()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }
    public void Setting()
    {
        spriteRenderer.size = new Vector2(FishingData.MiniGame_2_Data.AttackRange * 4f, SizeY);
    }

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
            if (IsObjectNearInfiniteLine(startPosition.position, endPosition.position, fishPosition.position, attackRange))

                FishController.Instance.ModifyHp(-attackDamage);

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
