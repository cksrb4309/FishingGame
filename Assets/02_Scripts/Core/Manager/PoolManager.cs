using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PoolManager : MonoBehaviour
{
    static PoolManager instance = null;
    [SerializeField] SerializedDictionary<ObjectPoolID, GameObject> objectPoolSetting;
    Dictionary<ObjectPoolID, object> pools = new Dictionary<ObjectPoolID, object>();
    public static void CreatePool<T>(ObjectPoolID id, T prefab, int initialSize = 5) where T : MonoBehaviour
    {
        if (!instance.pools.ContainsKey(id))
        {
            instance.pools[id] = new ObjectPool<T>(prefab, initialSize, instance.transform);
        }
    }
    public static void CreatePool<T>(ObjectPoolID id, int initialSize = 5) where T : Component
    {
        PoolManager poolManager = instance;

        if (poolManager == null) { Debug.LogError("[Error] : 풀매니저 초기화 오류."); return; }

        if (!poolManager.objectPoolSetting.ContainsKey(id) || poolManager.objectPoolSetting[id] == null) return;
        

        if (!poolManager.pools.ContainsKey(id))
        {
            var obj = poolManager.objectPoolSetting[id].GetComponent<T>();

            poolManager.pools[id] = new ObjectPool<T>(obj, initialSize, poolManager.transform);
        }
        else
        {
            Debug.Log("[Warning] : 오브젝트 풀 생성되어있음. " + id.ToString());
        }
    }
    public static T GetObj<T>(ObjectPoolID id) where T : MonoBehaviour
    {
        if (instance.pools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool_1)

            return pool_1.Pop();

        else
        {
            CreatePool<T>(id, 5);

            if (instance.pools.TryGetValue(id, out poolObj) && poolObj is ObjectPool<T> pool_2)

                return pool_2.Pop();

            else return null;
        }
    }
    public static void ReturnObj<T>(ObjectPoolID id, T obj) where T : MonoBehaviour
    {
        if (instance.pools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            pool.Push(obj);
        }
        else
        {
            Debug.LogWarning("ReturnObj Error : " + id.ToString());
        }
    }
    private void Awake()
    {
        instance = this;

        CreatePool<AttackAreaCircle>(ObjectPoolID.AttackArea, 4);
    }
}

public enum ObjectPoolID
{
    [InspectorName("1번 미니게임 공격 오브젝트")] AttackArea = 0,
    [InspectorName("3번 미니게임 공격 오브젝트")] AttackProjectile = 1,
    [InspectorName("4번 미니게임 회피 1")] FishPattern_1 = 2,
    [InspectorName("4번 미니게임 회피 2")] FishPattern_2 = 3,
    [InspectorName("5번 미니게임 공격 오브젝트 1")] AttackProjectile_5_1 = 4,
    [InspectorName("6번 미니게임 찌르기 오브젝트")] ThrushSlash = 5,
    
}