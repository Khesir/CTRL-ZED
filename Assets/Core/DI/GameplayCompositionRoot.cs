using UnityEngine;

/// <summary>
/// Composition root for gameplay-specific services.
/// Called by GameplayManager to wire up gameplay dependencies.
/// </summary>
public static class GameplayCompositionRoot
{
    public static void Configure(
        DIContainer container,
        EnemyManager enemyManager,
        WaveManager waveManager,
        LootManager lootManager,
        FollowerManager followerManager,
        DamageNumberService damageNumberService,
        GameplayUIController gameplayUI)
    {
        // Register gameplay services
        container.RegisterSingleton<IEnemyManager>(enemyManager);
        container.RegisterSingleton<IWaveManager>(waveManager);
        container.RegisterSingleton<ILootManager>(lootManager);
        container.RegisterSingleton<IFollowerManager>(followerManager);
        container.RegisterSingleton<IDamageNumberService>(damageNumberService);

        // Register UI
        container.RegisterSingleton(gameplayUI);

        // Also register with ServiceLocator for backward compatibility
        ServiceLocator.Register<IEnemyManager>(enemyManager);
        ServiceLocator.Register<ILootManager>(lootManager);
        ServiceLocator.Register<IFollowerManager>(followerManager);
        ServiceLocator.Register<IWaveManager>(waveManager);
        ServiceLocator.Register<IDamageNumberService>(damageNumberService);

        Debug.Log("[GameplayCompositionRoot] Gameplay services configured");
    }
}
