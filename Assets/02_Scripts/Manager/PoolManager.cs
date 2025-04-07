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
            Debug.LogWarning("Ǯ �Ŵ��� �ʱ�ȭ ���� ����"); return;
        }

        if (!poolManager.objectPoolSetting.ContainsKey(id) || poolManager.objectPoolSetting[id] == null)
        {
            Debug.LogError($"[ERROR] objectPoolSetting�� {id}�� �������� �ʰų� null�Դϴ�!"); return;
        }

        if (!poolManager.pools.ContainsKey(id))
        {
            Debug.Log("������Ʈ Ǯ ���� ����");
            try
            {
                var obj = poolManager.objectPoolSetting[id].GetComponent<T>();
                Debug.Log($"[DEBUG] {id}���� GetComponent<T>() ����: {obj}");

                poolManager.pools[id] = new ObjectPool<T>(obj, initialSize, poolManager.transform);
                Debug.Log($"[DEBUG] {id}�� ���� ObjectPool ���� �Ϸ�");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ERROR] ObjectPool ���� �� ���� �߻�! ID: {id}\n{e}");
            }
        }
        else
        {
            Debug.Log("������Ʈ Ǯ �����Ǿ� ����");
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
    [InspectorName("���� ����ü_1")] PlayerProjectile_1 = 0,
    [InspectorName("�� ����ü_1")] EnemyProjectile_1 = 1,
    [InspectorName("�ͷ� ����ü_1")] TurretProjectile_1 = 2,
}