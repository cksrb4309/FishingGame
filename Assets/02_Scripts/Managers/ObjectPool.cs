using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    Queue<T> pool = new Queue<T>();

    Transform parent;

    T prefab;

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
        }

        return obj;
    }

    public void Push(T obj)
    {
        pool.Enqueue(obj);
    }
}