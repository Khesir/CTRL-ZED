using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveService
{
    private readonly WaveConfig config;
    private readonly EnemySpawner spawner;

    private float spawnTimer;
    private int killCount;
    private bool isActive;

    public event Action OnWaveCompleted;
    public WaveService(WaveConfig config, EnemySpawner spawner)
    {
        this.config = config;
        this.spawner = spawner;
    }
    public void StartWave()
    {
        isActive = true;
        killCount = 0;
        spawnTimer = 0;

        GameplayManager.Instance.gameplayUI.timer.SetupTimer(config.attackTimer);
        GameplayManager.Instance.gameplayUI.timer.SetupAttackTimerDmaage(config.minAttackTimerDamage, config.maxAttackTimerDamage);
        GameplayManager.Instance.gameplayUI.timer.TriggerTimer();
        Debug.Log($"[WaveService] Wave started");
    }
    public void Update()
    {
        if (!isActive) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= config.spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        var enemyConfigs = config.enemyConfigs[UnityEngine.Random.Range(0, config.enemyConfigs.Count)];
        spawner.SpawnEnemy(enemyConfigs);
    }
    public void RegisterKill()
    {
        killCount++;
        if (killCount >= config.requiredKills)
        {
            isActive = false;
            OnWaveCompleted?.Invoke();
        }
    }

    public void StopWave() => isActive = false;
    public WaveConfig GetConfig() => config;
}
