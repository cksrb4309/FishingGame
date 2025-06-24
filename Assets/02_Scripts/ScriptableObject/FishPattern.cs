using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "FishPattern", menuName = "FishPattern/FishPattern")]
public class FishPattern : ScriptableObject
{
    [SerializeField] FishPatternData[] fishPatternDatas;
    [SerializeField] int startPattern = 0;
    FishPatternData current = null;
    bool isStart = true;
    public float SpawnPattern()
    {
        if (isStart)
        {
            isStart = false;

            current = fishPatternDatas[startPattern];
        }
        else
        {
            int nextPattern = current.NextPattern();
            
            current = fishPatternDatas[nextPattern];
        }
        current.SpawnPattern();

        return current.GetNextPatternDelay();
    }
    public void Init()
    {
        for (int i = 0; i < fishPatternDatas.Length; i++)
            PoolManager.CreatePool<FishPatternObj>(fishPatternDatas[i].GetPatternID(), 1);
    }
    public void Reset()
    {
        current = null;

        isStart = true;
    }
}

[Serializable]
public class FishPatternData
{
    [SerializeField] ObjectPoolID patternID;
    [SerializeField] Vector2 pivotPosition;
    [SerializeField] int damage;
    [SerializeField] float attackDelay;
    [SerializeField] float angle;
    [SerializeField] float nextPatternDelay;
    [SerializeField] bool useLocalPosition;
    [SerializeField] bool useRandomPosition;
    [SerializeField] bool useRandomAngle;
    [SerializeField] List<Vector2Int> nextPatternProbability;
    public int NextPattern()
    {
        int totalWeight = nextPatternProbability.Sum(p => p.y);
        int randomValue = Random.Range(0, totalWeight + 1);
        int cumulative = 0;

        foreach (var p in nextPatternProbability)
        {
            cumulative += p.y;

            if (randomValue < cumulative) return p.x;
        }
        
        return nextPatternProbability[nextPatternProbability.Count - 1].x;
    }
    public void SpawnPattern()
    {
        FishPatternObj fishPatternObj = PoolManager.GetObj<FishPatternObj>(patternID);

        fishPatternObj.Enable(damage, attackDelay);

        SetPosition(fishPatternObj.transform);
    }
    void SetPosition(Transform transform)
    {
        Vector2 spawnPosition = useLocalPosition ? UserMainController.Instance.Position : Camera.main.transform.position;

        spawnPosition += useRandomPosition ? Random.insideUnitCircle * pivotPosition.magnitude : pivotPosition;

        transform.position = spawnPosition;

        float angle = GetAngle();

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    float GetAngle()
    {
        if (useRandomAngle) return Random.Range(0f, 360);
        
        else return this.angle;
    }
    public float GetNextPatternDelay() => nextPatternDelay;
    public ObjectPoolID GetPatternID() => patternID;
}