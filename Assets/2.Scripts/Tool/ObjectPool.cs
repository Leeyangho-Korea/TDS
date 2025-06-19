using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;
    private Transform _parent;

    // 풀 생성자
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this._prefab = prefab;
        this._parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(_prefab, _parent);
        return obj;
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            T newObj = CreateNewObject();
            newObj.gameObject.SetActive(true);
            return newObj;
        }

        T obj = _pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}
