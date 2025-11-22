using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralized manager for object pools.
/// Register pools here for easy access throughout the game.
/// </summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<string, object> _pools = new();
    private Transform _poolContainer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Create container for pooled objects
        _poolContainer = new GameObject("PooledObjects").transform;
        _poolContainer.SetParent(transform);
    }

    public void CreatePool<T>(string poolId, T prefab, int initialSize, int maxSize = 100) where T : Component
    {
        if (_pools.ContainsKey(poolId))
        {
            Debug.LogWarning($"[PoolManager] Pool '{poolId}' already exists.");
            return;
        }

        var pool = new ObjectPool<T>(prefab, initialSize, _poolContainer, maxSize);
        _pools[poolId] = pool;

        Debug.Log($"[PoolManager] Created pool '{poolId}' with {initialSize} initial objects.");
    }

    public ObjectPool<T> GetPool<T>(string poolId) where T : Component
    {
        if (_pools.TryGetValue(poolId, out var pool))
        {
            return pool as ObjectPool<T>;
        }

        Debug.LogError($"[PoolManager] Pool '{poolId}' not found.");
        return null;
    }

    public T Spawn<T>(string poolId) where T : Component
    {
        var pool = GetPool<T>(poolId);
        return pool?.Get();
    }

    public T Spawn<T>(string poolId, Vector3 position, Quaternion rotation) where T : Component
    {
        var pool = GetPool<T>(poolId);
        return pool?.Get(position, rotation);
    }

    public void Despawn<T>(string poolId, T obj) where T : Component
    {
        var pool = GetPool<T>(poolId);
        pool?.Return(obj);
    }

    public void ClearPool(string poolId)
    {
        if (_pools.TryGetValue(poolId, out var pool))
        {
            // Use reflection to call Clear() since we don't know the type
            var clearMethod = pool.GetType().GetMethod("Clear");
            clearMethod?.Invoke(pool, null);
        }
    }

    public void ClearAllPools()
    {
        foreach (var poolId in _pools.Keys)
        {
            ClearPool(poolId);
        }
        _pools.Clear();
    }

    private void OnDestroy()
    {
        ClearAllPools();
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
