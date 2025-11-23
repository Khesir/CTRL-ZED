using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour, IWaveManager
{
    [Header("Debug Log : Don't touch")]
    [SerializeField] private EnemySpawner spawner;

    private int waveIndex = 0;
    public WaveService currentWave;
    public WaveService CurrentWave => currentWave;

    private List<WaveConfig> waveConfigs;

    // Cached service references
    private ISoundService soundService;
    private IEnemyManager enemyManager;
    private AttackTimer attackTimer;

    private void OnEnable()
    {
        SceneEventBus.Subscribe<EnemyDefeatedEvent>(OnEnemyDefeated);
    }

    private void OnDisable()
    {
        SceneEventBus.Unsubscribe<EnemyDefeatedEvent>(OnEnemyDefeated);
    }

    private void OnEnemyDefeated(EnemyDefeatedEvent evt)
    {
        // Replace direct ReportKill call - now triggered by event
        ReportKillInternal();
    }

    public void Initialize()
    {
        waveConfigs = null;
        waveIndex = 0;
        currentWave = null;

        enemyManager = ServiceLocator.Get<IEnemyManager>();
        attackTimer = ServiceLocator.Get<GameplayUIController>().timer;
        Debug.Log("[WaveManager] Initialized");
    }
    public void SetWaveConfig(List<WaveConfig> waveConfigs)
    {
        this.waveConfigs = waveConfigs;
    }
    private void Update()
    {
        currentWave?.Update();
    }
    public void StartNextWave()
    {
        if (waveIndex >= waveConfigs.Count)
        {
            enemyManager.KillAllEnemies(true);
            Debug.Log("[WaveManager] All waves completed.");

            // Publish event instead of direct UI call
            SceneEventBus.Publish(new WaveMessageEvent { Message = "All Waves Cleared!" });
            SceneEventBus.Publish(new AllWavesCompletedEvent
            {
                TotalWaves = waveConfigs.Count,
                TotalReward = 0
            });

            currentWave = null;
            return;
        }

        var config = waveConfigs[waveIndex];
        currentWave = new WaveService(config, spawner, attackTimer);

        currentWave.StartWave();

        // Publish events instead of direct UI calls
        SceneEventBus.Publish(new WaveMessageEvent { Message = $"Start Wave {waveIndex + 1}" });
        SceneEventBus.Publish(new WaveStartedEvent
        {
            WaveNumber = waveIndex + 1,
            TotalWaves = waveConfigs.Count,
            EnemyCount = config.requiredKills
        });
    }
    public int GetWaveIndex() => waveIndex;

    // Keep public for backward compatibility, but internal logic moved
    public void ReportKill()
    {
        ReportKillInternal();
    }

    private void ReportKillInternal()
    {
        if (currentWave == null || currentWave.IsComplete())
            return;

        currentWave.RegisterKill();
        var currentKills = currentWave.GetKillCount();
        var requiredKills = currentWave.GetRequiredKills();

        // Publish progress event instead of direct UI call
        SceneEventBus.Publish(new WaveProgressUpdatedEvent
        {
            CurrentKills = currentKills,
            RequiredKills = requiredKills,
            WaveIndex = waveIndex
        });

        if (currentWave.IsComplete())
        {
            OnWaveCompleted();
        }
    }

    private void OnWaveCompleted()
    {
        // Step 1: Kill remaining enemies
        enemyManager.KillAllEnemies(true);

        // Step 2: Publish wave cleared message
        SceneEventBus.Publish(new WaveMessageEvent { Message = "Wave Cleared" });

        // Step 3: Prepare and publish rewards
        waveIndex++;
        var loots = currentWave.GetConfig().waveRewards;
        bool isLastWave = waveIndex >= waveConfigs.Count;

        // Publish wave completed event with rewards
        SceneEventBus.Publish(new WaveCompletedEvent
        {
            WaveNumber = waveIndex,
            Rewards = loots,
            IsLastWave = isLastWave
        });

        // Step 4: Check if level is done or next wave
        if (!isLastWave)
        {
            SceneEventBus.Publish(new WaveMessageEvent { Message = "Next Wave Incoming" });
        }

        currentWave = null;
    }
    public void PauseWave(bool flag)
    {
        if (flag)
        {
            currentWave.StopWave();
        }
        else
        {
            currentWave.ResumeWave();
        }
    }
}
