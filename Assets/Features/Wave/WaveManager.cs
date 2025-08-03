using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private EnemySpawner spawner;

    private int waveIndex = 0;
    private WaveService currentWave;
    private List<WaveConfig> waveConfigs;

    public void Initialize(LevelData levelData)
    {
        currentLevelData = levelData;
        waveConfigs = levelData.waveSet.waves;
        waveIndex = 0;
        currentWave = null;
        // Add controls if its a campaign or endless here
        Debug.Log("[WaveManager] Initialized with level: " + currentLevelData.levelName);
    }
    private void Update()
    {
        currentWave?.Update();
    }
    public void StartNextWave()
    {
        if (waveIndex >= waveConfigs.Count)
        {
            Debug.Log("[WaveManager] All waves completed.");
            GameplayManager.Instance.gameplayUI.PushMessage("All Waves Cleared!");
            return;
        }

        var config = waveConfigs[waveIndex];
        currentWave = new WaveService(config, spawner);
        currentWave.OnWaveCompleted += OnWaveCompleted;

        currentWave.StartWave();
        GameplayManager.Instance.gameplayUI.PushMessage($"Start Wave {waveIndex + 1}");
    }
    private void OnWaveCompleted()
    {
        GameplayManager.Instance.gameplayUI.PushMessage("Wave Cleared");

        var loots = currentWave.GetConfig().waveRewards;
        var team = GameManager.Instance.TeamManager.GetActiveTeam();
        GameplayManager.Instance.gameplayUI.Complete("character", true, team[0].GetTeamName(), loots);

        waveIndex++;
        currentWave = null;

        GameplayManager.Instance.gameplayUI.starWaveButton.SetActive(true);
    }

    public void ReportKill()
    {
        currentWave?.RegisterKill();
    }
}
