using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDatabase", menuName = "Quest/QuestDatabase")]
public class QuestDatabase : ScriptableObject, IEnumerable<QuestInfo>
{
    [SerializeField] List<QuestInfo> questInfos;
    public IEnumerator<QuestInfo> GetEnumerator()
    {
        foreach (var quest in questInfos) yield return quest;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public QuestInfo GetQuestById(int id) => questInfos.Where(q => q.questId == id).First();
}
