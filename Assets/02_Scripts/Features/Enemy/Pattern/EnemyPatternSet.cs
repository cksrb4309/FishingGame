using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPatternSet", menuName = "Enemy/Pattern/EnemyPatternSet")]
public class EnemyPatternSet : ScriptableObject
{
    public List<EnemyPattern> enemyPatterns;
    
    public EnemyPattern GetPattern()
    {
        List<EnemyPattern> patterns = enemyPatterns.Where(p => p.IsMet()).ToList();

        return patterns[Random.Range(0, patterns.Count)];
    }
    public void Init()
    {

    }
}
