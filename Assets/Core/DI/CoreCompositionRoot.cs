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

        // Register managers (concrete types for backward compatibility)
        container.RegisterSingleton(gameManager);
        container.RegisterSingleton(gameStateManager);

        // Register sub-managers with interfaces for proper DI
        container.RegisterSingleton<IPlayerDataManager>(gameManager.GetPlayerDataManager());
        container.RegisterSingleton<IPlayerManager>(gameManager.GetPlayerManager());
        container.RegisterSingleton<ICharacterManager>(gameManager.GetCharacterManager());
        container.RegisterSingleton<ITeamManager>(gameManager.GetTeamManager());
        container.RegisterSingleton<IAntiVirusManager>(gameManager.GetAntiVirusManager());
        container.RegisterSingleton<ILevelManager>(gameManager.GetLevelManager());
        container.RegisterSingleton<IStatusEffectManager>(gameManager.GetStatusEffectManager());

        Debug.Log("[CoreCompositionRoot] Core services configured");
    }
}
