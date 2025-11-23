using UnityEngine;

/// <summary>
/// Example poolable projectile that auto-returns to pool after lifetime.
/// Extend or modify based on your projectile needs.
/// </summary>
public class PoolableProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private string poolId = "Projectiles";

    private float _timer;
    private Rigidbody2D _rb;
    private TrailRenderer _trail;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
    }

    public void OnSpawnFromPool()
    {
        _timer = lifetime;

        // Reset velocity
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }

        // Clear trail
        if (_trail != null)
        {
            _trail.Clear();
        }
    }

    public void OnReturnToPool()
    {
        // Reset state
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            ReturnToPool();
        }
    }

    public void Launch(Vector2 direction, float speed)
    {
        if (_rb != null)
        {
            _rb.velocity = direction.normalized * speed;
        }
    }

    public void ReturnToPool()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.Despawn(poolId, this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision - extend as needed
        // ReturnToPool();
    }
}
