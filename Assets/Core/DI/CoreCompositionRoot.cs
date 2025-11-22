using UnityEngine;

/// <summary>
/// Composition root for core/persistent services.
/// Called by GameInitiator to wire up all core dependencies.
/// </summary>
public static class CoreCompositionRoot
{
    public static void Configure(
        DIContainer container,
        SoundManager soundManager,
        InputService inputService,
        GameManager gameManager,
        GameStateManager gameStateManager)
    {
        // Register core services
        container.RegisterSingleton<ISoundService>(soundManager);
        container.RegisterSingleton<IInputService>(inputService);

        // Register managers
        container.RegisterSingleton(gameManager);
        container.RegisterSingleton(gameStateManager);

        // Register sub-managers from GameManager
        container.RegisterSingleton(gameManager.PlayerDataManager);
        container.RegisterSingleton(gameManager.PlayerManager);
        container.RegisterSingleton(gameManager.CharacterManager);
        container.RegisterSingleton(gameManager.TeamManager);
        container.RegisterSingleton(gameManager.AntiVirusManager);
        container.RegisterSingleton(gameManager.LevelManager);
        container.RegisterSingleton(gameManager.StatusEffectManager);

        Debug.Log("[CoreCompositionRoot] Core services configured");
    }
}
