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

    public void Initialize()
    {
        waveConfigs = null;
        waveIndex = 0;
        currentWave = null;

        enemyManager = ServiceLocator.Get<IEnemyManager>();
        // Add controls if its a campaign or endless here
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
    public async void StartNextWave()
    {
        if (waveIndex >= waveConfigs.Count)
        {
            enemyManager.KillAllEnemies(true);
            Debug.Log("[WaveManager] All waves completed.");
            await GameplayManager.Instance.GameplayUI.PushMessageAsync("All Waves Cleared!");
            currentWave = null;
            return;
        }

        var config = waveConfigs[waveIndex];
        currentWave = new WaveService(config, spawner);

        currentWave.StartWave();
        await GameplayManager.Instance.GameplayUI.PushMessageAsync($"Start Wave {waveIndex + 1}");
        GameplayManager.Instance.GameplayUI.starWaveButton.SetActive(false);
    }
    public int GetWaveIndex() => waveIndex;

    public void ReportKill()
    {
        if (currentWave.IsComplete())
            return;

        currentWave?.RegisterKill();
        var currentKills = currentWave.GetKillCount();
        var requiredKills = currentWave.GetRequiredKills();

        GameplayManager.Instance.GameplayUI.waveUI.UpdateSlider(currentKills, requiredKills, waveIndex);
        if (currentWave.IsComplete())
        {
            OnWaveCompleted();
        }
    }
    // Todo: Wave on Complete Checker is broken and need urgently to be addressed
    private async void OnWaveCompleted()
    {

        // Step 1: Kill remaining enemies after message is done
        enemyManager.KillAllEnemies(true);

        // Step 2: Show "Wave Cleared"
        await GameplayManager.Instance.GameplayUI.PushMessageAsync("Wave Cleared");

        // Step 3: Prepare rewards
        waveIndex++;
        var loots = currentWave.GetConfig().waveRewards;
        foreach (var loot in loots)
        {
            GameplayManager.Instance.GameplayUI.lootHolder.AddAmount(loot);
        }

        // Step 4: Check if level is done or next wave
        if (waveIndex >= waveConfigs.Count)
        {
            // Wait for "Level Complete" panel to finish showing
            // GameplayManager.Instance.endGameState = GameplayManager.GameplayEndGameState.LevelComplete;
            // await GameplayManager.Instance.SetState(GameplayManager.GameplayState.End);
        }
        else
        {
            // Optionally show another message like “Next Wave Starting”
            await GameplayManager.Instance.GameplayUI.PushMessageAsync("Next Wave Incoming");

            // Then wait before starting next wave
            // await GameplayManager.Instance.SetState(GameplayManager.GameplayState.Start);
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
