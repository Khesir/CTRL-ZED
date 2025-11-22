using UnityEngine;

/// <summary>
/// Gameplay events related to loot collection.
/// Publish via SceneEventBus - destroyed with gameplay scene.
/// </summary>

public struct LootCollectedEvent
{
    public LootDropData Data;
    public Vector3 Position;
}

public struct LootDroppedEvent
{
    public LootDropData Data;
    public Vector3 Position;
}

public struct LootRewardAddedEvent
{
    public LootDropData Data;
}
