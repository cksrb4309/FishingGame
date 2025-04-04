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
    public static void CreatePool<T>(ObjectPoolID id, int initialSize = 5, T t = null) where T : Component
    {
        PoolManager poolManager = instance;

        if (poolManager == null)
        {
            Debug.LogWarning("풀 매니저 초기화 문제 있음"); return;
        }

        if (!poolManager.objectPoolSetting.ContainsKey(id) || poolManager.objectPoolSetting[id] == null)
        {
            Debug.LogError($"[ERROR] objectPoolSetting에 {id}가 존재하지 않거나 null입니다!"); return;
        }

        if (!poolManager.pools.ContainsKey(id))
        {
            Debug.Log("오브젝트 풀 생성 요함");
            try
            {
                var obj = poolManager.objectPoolSetting[id].GetComponent<T>();
                Debug.Log($"[DEBUG] {id}에서 GetComponent<T>() 성공: {obj}");

                poolManager.pools[id] = new ObjectPool<T>(obj, initialSize, poolManager.transform);
                Debug.Log($"[DEBUG] {id}에 대한 ObjectPool 생성 완료");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ERROR] ObjectPool 생성 중 예외 발생! ID: {id}\n{e}");
            }
        }
        else
        {
            Debug.Log("오브젝트 풀 생성되어 있음");
        }
    }
    public static T GetObj<T>(ObjectPoolID id) where T : MonoBehaviour
    {
        if (instance.pools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool)
        
            return pool.Pop();

        Debug.LogWarning("GetObj Error : " + id.ToString());

        return null;
    }
    public static void ReturnObj<T>(ObjectPoolID id, T obj) where T : MonoBehaviour
    {
        Debug.Log("Return Obj : " + obj.gameObject.name);

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
    }
}

public enum ObjectPoolID
{
    [InspectorName("유저 투사체_1")] PlayerProjectile_1 = 0,
    [InspectorName("적 투사체_1")] EnemyProjectile_1 = 1,
    [InspectorName("터렛 투사체_1")] TurretProjectile_1 = 2,
}