using UnityEngine;

/// <summary>
/// Gameplay events related to combat.
/// Publish via SceneEventBus - destroyed with gameplay scene.
/// </summary>

public struct DamageDealtEvent
{
    public int SourceId;
    public int TargetId;
    public int Damage;
    public bool IsCritical;
    public Vector3 Position;
}

public struct EntityDeathEvent
{
    public int EntityId;
    public bool IsEnemy;
    public Vector3 Position;
}

public struct HealEvent
{
    public int TargetId;
    public int Amount;
    public Vector3 Position;
}

public struct StatusEffectAppliedEvent
{
    public int TargetId;
    public string EffectName;
    public float Duration;
}

public struct StatusEffectRemovedEvent
{
    public int TargetId;
    public string EffectName;
}
