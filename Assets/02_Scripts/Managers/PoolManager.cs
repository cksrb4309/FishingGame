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
    public static void CreatePool<T>(ObjectPoolID id, int initialSize = 5) where T : MonoBehaviour
    {
        if (!instance.pools.ContainsKey(id))
        {
            instance.pools[id] = new ObjectPool<T>(instance.objectPoolSetting[id].GetComponent<T>(), initialSize, instance.transform);
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
    PlayerBullet_1 = 0,
    EnemyBullet_1 = 1,
}