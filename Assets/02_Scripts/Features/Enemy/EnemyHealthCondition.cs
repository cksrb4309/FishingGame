using UnityEngine;

[CreateAssetMenu(fileName = "EnemyHealthCondition", menuName = "Enemy/Pattern/Condition/EnemyHealthCondition")]
public class EnemyHealthCondition : PatternCodition
{
    [Range(0f, 1f)]
    [Tooltip("체력 비율 (0 ~ 1)")]
    [SerializeField] private float healthThreshold = 0.5f;

    [Tooltip("체력 조건 판정 방식 (이하 / 이상)")]
    [SerializeField] private ComparisonType comparison = ComparisonType.LessOrEqual;

    public override bool IsMet()
    {
        float healthRatio = EnemyData.Current.GetHpRatio();

        switch (comparison)
        {
            case ComparisonType.LessOrEqual: return healthRatio <= healthThreshold;

            case ComparisonType.GreaterOrEqual: return healthRatio >= healthThreshold;

            default: return false;
        }
    }
}
