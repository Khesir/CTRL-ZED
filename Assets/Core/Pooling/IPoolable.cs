/// <summary>
/// Interface for objects that can be pooled.
/// </summary>
public interface IPoolable
{
    void OnSpawnFromPool();
    void OnReturnToPool();
}
