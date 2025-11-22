/// <summary>
/// Core events related to game state changes.
/// Publish via CoreEventBus - persists across scenes.
/// </summary>

public struct GameStateChangedEvent
{
    public GameState PreviousState;
    public GameState NewState;
}

public struct GamePausedEvent
{
    public bool IsPaused;
}

public struct GameInitializedEvent
{
    public bool Success;
}
