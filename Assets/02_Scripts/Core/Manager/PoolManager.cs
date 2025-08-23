using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PoolManager : MonoBehaviour
{
    static PoolManager instance = null;

    [SerializeField] SerializedDictionary<ObjectPoolID, GameObject> objectPoolSetting;

    Dictionary<ObjectPoolID, object> idPools = new Dictionary<ObjectPoolID, object>();
    Dictionary<int, object> objPools = new Dictionary<int, object>();
    public static void CreatePool<T>(ObjectPoolID id, T prefab, int initialSize = 5) where T : MonoBehaviour
    {
        if (!instance.idPools.ContainsKey(id))
        {
            instance.idPools[id] = new ObjectPool<T>(prefab, initialSize, instance.transform);
        }
    }
    public static void CreatePool<T>(ObjectPoolID id, int initialSize = 5) where T : Component
    {
        PoolManager poolManager = instance;

        if (poolManager == null) { Debug.LogError("[Error] : 풀매니저 초기화 오류."); return; }

        if (!poolManager.objectPoolSetting.ContainsKey(id) || poolManager.objectPoolSetting[id] == null) return;
        

        if (!poolManager.idPools.ContainsKey(id))
        {
            var obj = poolManager.objectPoolSetting[id].GetComponent<T>();

            poolManager.idPools[id] = new ObjectPool<T>(obj, initialSize, poolManager.transform);
        }
        else
        {
            Debug.Log("[Warning] : 오브젝트 풀 생성되어있음. " + id.ToString());
        }
    }
    public static void CreatePool<T>(GameObject obj, int initialSize = 5) where T : MonoBehaviour
    {
        PoolManager poolManager = instance;

        if (poolManager == null) { Debug.LogError("[Error] : 풀매니저 초기화 오류."); return; }

        if (!poolManager.objPools.ContainsKey(obj.GetInstanceID()))
        {
            T t = obj.GetComponent<T>();

            poolManager.objPools[obj.GetInstanceID()] = new ObjectPool<T>(t, initialSize, poolManager.transform);
        }
        else
        {
            Debug.Log("[Warning] : 오브젝트 풀 생성되어있음. " + obj.name.ToString());
        }
    }
    public static T GetObj<T>(ObjectPoolID id) where T : MonoBehaviour
    {
        if (instance.idPools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool_1)
        {
            return pool_1.Pop();
        }
        else
        {
            CreatePool<T>(id, 5);

            if (instance.idPools.TryGetValue(id, out poolObj) && poolObj is ObjectPool<T> pool_2)

                return pool_2.Pop();

            else return null;
        }
    }
    public static T GetObj<T>(GameObject obj) where T : MonoBehaviour
    {
        int instanceId = obj.GetInstanceID();

        if (instance.objPools.TryGetValue(instanceId, out object poolObj) && poolObj is ObjectPool<T> pool_1)
        {
            return pool_1.Pop();
        }
        else
        {
            CreatePool<T>(obj, 5);

            if (instance.objPools.TryGetValue(instanceId, out poolObj) && poolObj is ObjectPool<T> pool_2)

                return pool_2.Pop();

            else return null;
        }
    }
    public static void ReturnObj<T>(ObjectPoolID id, T obj) where T : MonoBehaviour
    {
        if (instance.idPools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            pool.Push(obj);
        }
        else
        {
            Debug.LogWarning("ReturnObj Error : " + id.ToString());
        }
    }
    public static void ReturnObj<T>(T obj) where T : MonoBehaviour
    {
        if (instance.objPools.TryGetValue(obj.GetInstanceID(), out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            pool.Push(obj);
        }
        else
        {
            Debug.LogWarning("ReturnObj Error : " + obj.name.ToString());
        }
    }
    private void Awake()
    {
        instance = this;
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