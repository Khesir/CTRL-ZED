using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public List<Wave> waves;
    public int waveNumber;
    public Transform minPos;
    public Transform maxPos;

    public int playerKillCount;
    public int waveLevel = 1;
    public bool startWave = false;
    // Update is called once per frame
    public event Action ReportedKill;
    public async UniTask Initialize(List<Wave> waves = null)
    {
        playerKillCount = 0;
        waveNumber = 0;
        waveLevel = 1;
        this.waves = waves;
        await UniTask.CompletedTask;
    }
    void Update()
    {
        if (startWave && GameplayManager.Instance.globalTargetPlayer != null)
        {
            GameplayManager.Instance.gameplayUI.timer.TriggerTimer(); // Attack Timer
            // Progress to next wave when enough kills are made
            if (playerKillCount >= waves[waveNumber].requiredKills)
            {
                ProgressToNextWave();
                return;
            }

            // Handle enemy spawning
            waves[waveNumber].spawntTimer += Time.deltaTime;
            if (waves[waveNumber].spawntTimer >= waves[waveNumber].spawnInterval)
            {
                waves[waveNumber].spawntTimer = 0;
                SpawnEnemy();
            }
        }
    }
    private void ProgressToNextWave()
    {
        KillRemainingEnemies();
        waveNumber++;
        startWave = false;
        if (waveNumber >= waves.Count)
        {
            var team = GameManager.Instance.TeamManager.GetActiveTeam();
            var loots = waves[waveNumber - 1].waveRewards;
            GameplayManager.Instance.gameplayUI.Complete("character", true, team[0].GetTeamName(), loots);
            return;
        }
        WaveAnnouncement("Wave Clear");
        waveLevel++;
        playerKillCount = 0;
        // Increase wave number or loop back
        // if (waveNumber >= waves.Count)
        // {
        //     waveNumber = 0;
        // }
        // Increase difficulty
        // if (waves[waveNumber].spawnInterval > 0.3f)
        // {
        //     waves[waveNumber].spawnInterval *= 0.9f;
        // }
        GameplayManager.Instance.gameplayUI.timer.SetupTimer(waves[waveNumber].attackTimer);
        GameplayManager.Instance.gameplayUI.starWaveButton.SetActive(true);
    }
    private void SpawnEnemy()
    {
        var wave = waves[waveNumber];
        var randomEnemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Count)];
        var go = Instantiate(randomEnemyPrefab, RandomSpawnPoint(), transform.rotation);
        var enemyFollowTarget = go.GetComponent<EnemyFollow>();
        enemyFollowTarget.target = GameplayManager.Instance.globalTargetPlayer;
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;
        if (Random.Range(0, 1f) > 0.5)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            if (Random.Range(0f, 1f) > 0.5)
            {
                spawnPoint.y = minPos.position.y;

            }
            else
            {
                spawnPoint.y = maxPos.position.y;

            }
        }
        else
        {
            spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
            if (Random.Range(0f, 1f) > 0.5)
            {
                spawnPoint.x = minPos.position.x;

            }
            else
            {
                spawnPoint.x = maxPos.position.x;
            }
        }

        return spawnPoint;
    }
    public void StartWave()
    {
        //Change Timer Settings
        GameplayManager.Instance.gameplayUI.timer.SetupTimer(waves[waveNumber].attackTimer);
        GameplayManager.Instance.gameplayUI.timer.SetupAttackTimerDmaage(waves[waveNumber].minAttackTimerDamage, waves[waveNumber].maxAttackTimerDamage);

        startWave = true;
        WaveAnnouncement($"Start Wave {waveLevel}");
        GameplayManager.Instance.gameplayUI.starWaveButton.SetActive(false);
        ReportedKill?.Invoke();
    }
    public void ReportKill(int count)
    {
        playerKillCount += count;
        ReportedKill?.Invoke();
    }
    private void KillRemainingEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(notPlayer: true);
        }
    }
    private void WaveAnnouncement(string announcemet)
    {
        GameplayManager.Instance.gameplayUI.PushMessage(announcemet);
    }
}
