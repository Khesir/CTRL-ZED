using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveService
{
    private readonly WaveConfig config;
    private readonly EnemySpawner spawner;
    private readonly AttackTimer timer;

    private float spawnTimer;
    private int killCount;
    private bool isActive;
    private bool isComplete = false;

    public WaveService(WaveConfig config, EnemySpawner spawner, AttackTimer timer)
    {
        this.config = config;
        this.spawner = spawner;
        this.timer = timer;
    }

    public void StartWave()
    {
        isActive = true;
        killCount = 0;
        spawnTimer = 0;

        timer.SetupTimer(config.attackTimer);
        timer.SetupAttackTimerDmaage(config.minAttackTimerDamage, config.maxAttackTimerDamage);
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
        timer.TriggerTimer();
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
            isComplete = true;
        }
    }

    public void StopWave() => isActive = false;
    public void ResumeWave() => isActive = true;
    public WaveConfig GetConfig() => config;
    public int GetKillCount() => killCount;
    public int GetRequiredKills() => config.requiredKills;
    public bool IsComplete() => isComplete;
}
