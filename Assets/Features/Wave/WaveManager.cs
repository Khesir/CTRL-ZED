using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Debug Log : Don't touch")]
    [SerializeField] private EnemySpawner spawner;

    private int waveIndex = 0;
    public WaveService currentWave;
    private List<WaveConfig> waveConfigs;

    public void Initialize()
    {
        waveConfigs = null;
        waveIndex = 0;
        currentWave = null;
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
            GameplayManager.Instance.enemyManager.KillAllEnemies(true);
            Debug.Log("[WaveManager] All waves completed.");
            await GameplayManager.Instance.gameplayUI.PushMessageAsync("All Waves Cleared!");
            currentWave = null;
            return;
        }

        var config = waveConfigs[waveIndex];
        currentWave = new WaveService(config, spawner);

        currentWave.StartWave();
        await GameplayManager.Instance.gameplayUI.PushMessageAsync($"Start Wave {waveIndex + 1}");
        GameplayManager.Instance.gameplayUI.starWaveButton.SetActive(false);
    }
    public int GetWaveIndex() => waveIndex;

    public void ReportKill()
    {
        if (currentWave.IsComplete())
            return;

        currentWave?.RegisterKill();
        var currentKills = currentWave.GetKillCount();
        var requiredKills = currentWave.GetRequiredKills();

        GameplayManager.Instance.gameplayUI.waveUI.UpdateSlider(currentKills, requiredKills, waveIndex);
        if (currentWave.IsComplete())
        {
            OnWaveCompleted();
        }
    }
    private async void OnWaveCompleted()
    {

        // Step 1: Kill remaining enemies after message is done
        GameplayManager.Instance.enemyManager.KillAllEnemies(true);

        // Step 2: Show "Wave Cleared"
        await GameplayManager.Instance.gameplayUI.PushMessageAsync("Wave Cleared");

        // Step 3: Prepare rewards
        waveIndex++;
        var loots = currentWave.GetConfig().waveRewards;
        foreach (var loot in loots)
        {
            GameplayManager.Instance.gameplayUI.lootHolder.AddAmount(loot);
        }

        // Step 4: Check if level is done or next wave
        if (waveIndex >= waveConfigs.Count)
        {
            // Wait for "Level Complete" panel to finish showing
            GameplayManager.Instance.endGameState = GameplayManager.GameplayEndGameState.LevelComplete;
            await GameplayManager.Instance.SetState(GameplayManager.GameplayState.End);
        }
        else
        {
            // Optionally show another message like “Next Wave Starting”
            await GameplayManager.Instance.gameplayUI.PushMessageAsync("Next Wave Incoming");

            // Then wait before starting next wave
            await GameplayManager.Instance.SetState(GameplayManager.GameplayState.Start);
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
