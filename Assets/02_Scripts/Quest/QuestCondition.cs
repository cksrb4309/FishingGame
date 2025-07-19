[System.Serializable]
public abstract class QuestCondition
{
    public QuestConditionType conditionType;
    public abstract bool IsMet();
}