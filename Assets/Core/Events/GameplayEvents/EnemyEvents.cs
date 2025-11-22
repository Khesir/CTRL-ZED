using UnityEngine;

/// <summary>
/// Gameplay events related to enemies.
/// Publish via SceneEventBus - destroyed with gameplay scene.
/// </summary>

public struct EnemySpawnedEvent
{
    public int EnemyId;
    public string EnemyType;
    public Vector3 Position;
}

public struct EnemyDefeatedEvent
{
    public int EnemyId;
    public Vector3 Position;
}

public struct EnemyReachedGoalEvent
{
    public int EnemyId;
    public int DamageToPlayer;
}
