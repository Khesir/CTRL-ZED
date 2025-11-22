/// <summary>
/// Core events related to settings changes.
/// Publish via CoreEventBus - persists across scenes.
/// </summary>

public struct AudioSettingsChangedEvent
{
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;
}

public struct DisplaySettingsChangedEvent
{
    public bool Fullscreen;
    public int ResolutionWidth;
    public int ResolutionHeight;
}
