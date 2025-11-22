using System.Collections.Generic;

/// <summary>
/// Gameplay events related to wave system.
/// Publish via SceneEventBus - destroyed with gameplay scene.
/// </summary>

public struct WaveStartedEvent
{
    public int WaveNumber;
    public int TotalWaves;
    public int EnemyCount;
}

public struct WaveCompletedEvent
{
    public int WaveNumber;
    public List<LootDropData> Rewards;
    public bool IsLastWave;
}

public struct AllWavesCompletedEvent
{
    public int TotalWaves;
    public int TotalReward;
}

public struct WaveProgressUpdatedEvent
{
    public int CurrentKills;
    public int RequiredKills;
    public int WaveIndex;
}

public struct WaveMessageEvent
{
    public string Message;
}
