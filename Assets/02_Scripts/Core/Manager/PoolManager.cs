using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private Dictionary<ObjectPoolID, GameObject> objectPoolSetting;

    // ID 기반 풀
    private Dictionary<ObjectPoolID, object> idPools = new Dictionary<ObjectPoolID, object>();
    // Prefab 기반 풀
    private Dictionary<GameObject, object> prefabPools = new Dictionary<GameObject, object>();

    #region Create Pool

    // ID 기반 풀 생성 (직접 prefab 제공)
    public static void CreatePool<T>(ObjectPoolID id, T prefab, int initialSize = 5) where T : MonoBehaviour
    {
        if (!Instance.idPools.ContainsKey(id))
        {
            Instance.idPools[id] = new ObjectPool<T>(prefab, initialSize, Instance.transform);
        }
    }

    // ID 기반 풀 생성 (objectPoolSetting 사용)
    public static void CreatePool<T>(ObjectPoolID id, int initialSize = 5) where T : Component
    {
        if (!Instance.objectPoolSetting.ContainsKey(id) || Instance.objectPoolSetting[id] == null) return;
        if (!Instance.idPools.ContainsKey(id))
        {
            var obj = Instance.objectPoolSetting[id].GetComponent<T>();
            Instance.idPools[id] = new ObjectPool<T>(obj, initialSize, Instance.transform);
        }
        else
        {
            Debug.Log("[Warning] : 오브젝트 풀 이미 생성됨 - " + id.ToString());
        }
    }

    // Prefab 기반 풀 생성
    public static void CreatePool<T>(T prefab, int initialSize = 5) where T : MonoBehaviour
    {
        if (prefab == null) return;

        if (!Instance.prefabPools.ContainsKey(prefab.gameObject))
        {
            var pool = new ObjectPool<T>(prefab.gameObject, prefab, initialSize, Instance.transform);
            Instance.prefabPools[prefab.gameObject] = pool;
        }
    }

    #endregion

    #region Get Object

    // ID 기반 가져오기
    public static T GetObj<T>(ObjectPoolID id) where T : MonoBehaviour
    {
        if (Instance.idPools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            return pool.Pop();
        }
        else
        {
            CreatePool<T>(id, 5);
            if (Instance.idPools.TryGetValue(id, out poolObj) && poolObj is ObjectPool<T> pool2)
                return pool2.Pop();
            return null;
        }
    }

    // Prefab 기반 가져오기
    public static T GetObj<T>(T prefab, int initialSize = 5) where T : MonoBehaviour
    {
        if (prefab == null) return null;

        if (Instance.prefabPools.TryGetValue(prefab.gameObject, out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            return pool.Pop();
        }
        else
        {
            CreatePool(prefab, initialSize);
            if (Instance.prefabPools.TryGetValue(prefab.gameObject, out poolObj) && poolObj is ObjectPool<T> pool2)
                return pool2.Pop();
            return null;
        }
    }

    #endregion

    #region Return Object

    // ID 기반 반환
    public static void ReturnObj<T>(ObjectPoolID id, T obj) where T : MonoBehaviour
    {
        if (Instance.idPools.TryGetValue(id, out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            pool.Push(obj);
        }
        else
        {
            Debug.LogWarning("ReturnObj Error : " + id.ToString());
            Destroy(obj.gameObject);
        }
    }

    // Prefab 기반 반환
    public static void ReturnObj<T>(T obj) where T : MonoBehaviour
    {
        if (obj == null) return;

        var pooled = obj.GetComponent<PooledObject>();
        if (pooled == null)
        {
            Debug.LogWarning("ReturnObj Error: PooledObject 없음, Destroy 처리");
            Destroy(obj.gameObject);
            return;
        }

        if (Instance.prefabPools.TryGetValue(pooled.prefab, out object poolObj) && poolObj is ObjectPool<T> pool)
        {
            pool.Push(obj);
        }
        else
        {
            Debug.LogWarning("ReturnObj Error: 풀을 찾을 수 없음, Destroy 처리");
            Destroy(obj.gameObject);
        }
    }

    #endregion
}

// ---------------------------------------------
// ObjectPoolID Enum
// ---------------------------------------------
public enum ObjectPoolID
{
    [InspectorName("1번 미니게임 공격 오브젝트")] AttackArea = 0,
    [InspectorName("3번 미니게임 공격 오브젝트")] AttackProjectile = 1,
    [InspectorName("4번 미니게임 회피 1")] FishPattern_1 = 2,
    [InspectorName("4번 미니게임 회피 2")] FishPattern_2 = 3,
    [InspectorName("5번 미니게임 공격 오브젝트 1")] AttackProjectile_5_1 = 4,
    [InspectorName("6번 미니게임 찌르기 오브젝트")] ThrushSlash = 5,
}