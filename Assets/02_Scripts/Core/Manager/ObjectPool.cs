using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new Queue<T>();
    private Transform parent;
    private T prefab;



    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);

            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public ObjectPool(GameObject prefab_1, T prefab_2, int initialSize, Transform parent = null)
    {
        prefab = prefab_2;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab_2, parent);
            AttachPooledObject(obj);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }


    private void AttachPooledObject(T obj)
    {
        var pooled = obj.GetComponent<PooledObject>();
        if (pooled == null)
        {
            pooled = obj.gameObject.AddComponent<PooledObject>();
        }
        pooled.prefab = prefab.gameObject;
    }

    public T Pop()
    {
        T obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(prefab, parent);
            AttachPooledObject(obj);
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}