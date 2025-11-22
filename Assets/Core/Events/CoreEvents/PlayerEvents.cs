/// <summary>
/// Core events related to player data changes.
/// Publish via CoreEventBus - persists across scenes.
/// </summary>

public struct PlayerDataChangedEvent
{
    public int Coins;
    public int Level;
}

public struct PlayerHealthChangedEvent
{
    public int CurrentHealth;
    public int MaxHealth;
}

public struct PlayerLevelUpEvent
{
    public int NewLevel;
    public int PreviousLevel;
}

public struct PlayerResourceChangedEvent
{
    public ResourceType ResourceType;
    public int NewAmount;
    public int Delta;
}

public struct PlayerDrivesChangedEvent
{
    public int CurrentDrives;
    public int MaxDrives;
}
