using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic object pool for Unity components.
/// Reduces garbage collection by reusing objects.
/// </summary>
public class ObjectPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Queue<T> _pool = new();
    private readonly Transform _parent;
    private readonly int _maxSize;

    public int PooledCount => _pool.Count;
    public int ActiveCount { get; private set; }

    public ObjectPool(T prefab, int initialSize, Transform parent = null, int maxSize = 100)
    {
        _prefab = prefab;
        _parent = parent;
        _maxSize = maxSize;

        // Pre-warm the pool
        for (int i = 0; i < initialSize; i++)
        {
            _pool.Enqueue(CreateNew());
        }
    }

    private T CreateNew()
    {
        var obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T Get()
    {
        T obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = CreateNew();
        }

        obj.gameObject.SetActive(true);
        ActiveCount++;

        // Call IPoolable if implemented
        if (obj is IPoolable poolable)
        {
            poolable.OnSpawnFromPool();
        }

        return obj;
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        var obj = Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public void Return(T obj)
    {
        if (obj == null) return;

        // Call IPoolable if implemented
        if (obj is IPoolable poolable)
        {
            poolable.OnReturnToPool();
        }

        obj.gameObject.SetActive(false);
        ActiveCount--;

        // Only return to pool if under max size
        if (_pool.Count < _maxSize)
        {
            _pool.Enqueue(obj);
        }
        else
        {
            Object.Destroy(obj.gameObject);
        }
    }

    public void Clear()
    {
        while (_pool.Count > 0)
        {
            var obj = _pool.Dequeue();
            if (obj != null)
            {
                Object.Destroy(obj.gameObject);
            }
        }
        ActiveCount = 0;
    }
}
