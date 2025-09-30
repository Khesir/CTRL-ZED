using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Debug Log : Don't touch")]
    [SerializeField] private EnemySpawner spawner;

    private int waveIndex = 0;
    private WaveService currentWave;
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
    public void StartNextWave()
    {
        if (waveIndex >= waveConfigs.Count)
        {
            GameplayManager.Instance.enemyManager.KillAllEnemies(true);
            Debug.Log("[WaveManager] All waves completed.");
            GameplayManager.Instance.gameplayUI.PushMessage("All Waves Cleared!");
            return;
        }

        var config = waveConfigs[waveIndex];
        currentWave = new WaveService(config, spawner);

        currentWave.StartWave();
        GameplayManager.Instance.gameplayUI.PushMessage($"Start Wave {waveIndex + 1}");
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
    private void OnWaveCompleted()
    {
        GameplayManager.Instance.gameplayUI.PushMessage("Wave Cleared");
        GameplayManager.Instance.enemyManager.KillAllEnemies(true);

        waveIndex++;
        // Conditional checks if level has reach the wave index or tthere is next level
        if (waveIndex >= waveConfigs.Count)
        {
            var loots = currentWave.GetConfig().waveRewards;
            foreach (var loot in loots)
            {
                GameplayManager.Instance.gameplayUI.lootHolder.AddAmount(loot);
            }
            var team = GameManager.Instance.TeamManager.GetActiveTeam();
            // This gets triggered to send a ui to say level complete
            GameplayManager.Instance.gameplayUI.Complete("character", true, team[0].GetTeamName());
        }
        else
        {
            var loots = currentWave.GetConfig().waveRewards;
            foreach (var loot in loots)
            {
                GameplayManager.Instance.gameplayUI.lootHolder.AddAmount(loot);
            }
            GameplayManager.Instance.gameplayUI.starWaveButton.SetActive(true);
        }
        currentWave = null;
    }
}
