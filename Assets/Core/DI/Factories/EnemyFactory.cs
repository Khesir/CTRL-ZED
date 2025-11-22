using UnityEngine;

/// <summary>
/// Factory for creating enemies with proper dependency injection.
/// </summary>
public class EnemyFactory
{
    private readonly IEnemyManager _enemyManager;
    private readonly ILootManager _lootManager;
    private readonly ISoundService _soundService;
    private readonly IWaveManager _waveManager;

    public EnemyFactory(
        IEnemyManager enemyManager,
        ILootManager lootManager,
        ISoundService soundService,
        IWaveManager waveManager)
    {
        _enemyManager = enemyManager;
        _lootManager = lootManager;
        _soundService = soundService;
        _waveManager = waveManager;
    }

    public EnemyService Create(GameObject prefab, Vector3 position, EnemyConfig config)
    {
        var enemyGO = Object.Instantiate(prefab, position, Quaternion.identity);
        var enemyService = enemyGO.GetComponent<EnemyService>();

        if (enemyService != null)
        {
            enemyService.Initialize(config);
        }

        return enemyService;
    }
}
