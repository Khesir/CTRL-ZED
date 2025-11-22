using UnityEngine;

/// <summary>
/// Gameplay events related to character deployment and management.
/// Publish via SceneEventBus - destroyed with gameplay scene.
/// </summary>

public struct CharacterDeployedEvent
{
    public int CharacterId;
    public Vector3 Position;
    public string CharacterName;
}

public struct CharacterRemovedEvent
{
    public int CharacterId;
    public Vector3 Position;
}

public struct CharacterSkillUsedEvent
{
    public int CharacterId;
    public string SkillName;
    public float Cooldown;
}

public struct TeamSwitchedEvent
{
    public int PreviousTeamIndex;
    public int NewTeamIndex;
}
