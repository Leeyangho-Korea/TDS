using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    // 풀 생성자
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(prefab, parent);
        return obj;
    }

    public T Get()
    {
        if (pool.Count == 0)
        {
            T newObj = CreateNewObject();
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
