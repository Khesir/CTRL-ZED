using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawntTimer;
        public float spawnInterval;
        public int enemiesPerwave;
        public int spawnedEnemyCount;
    }
    public List<Wave> waves;
    public int waveNumber;
    public Transform minPos;
    public Transform maxPos;

    // Update is called once per frame
    void Update()
    {
        waves[waveNumber].spawntTimer += Time.deltaTime;
        if (waves[waveNumber].spawntTimer >= waves[waveNumber].spawnInterval)
        {
            waves[waveNumber].spawntTimer = 0;
            SpawnEnemy();
        }
        if (waves[waveNumber].spawnedEnemyCount >= waves[waveNumber].enemiesPerwave)
        {
            waves[waveNumber].spawnedEnemyCount = 0;
            if (waves[waveNumber].spawnInterval > 0.3f)
            {
                waves[waveNumber].spawnInterval *= 0.9f;
            }
            waveNumber++;
        }
        // ENd of the wave flag
        if (waveNumber >= waves.Count)
        {
            waveNumber = 0;
        }
    }

    private void SpawnEnemy()
    {
        var go = Instantiate(waves[waveNumber].enemyPrefab, RandomSpawnPoint(), transform.rotation);
        var enemyFollowTarget = go.GetComponent<EnemyFollow>();
        enemyFollowTarget.target = GameplayManager.Instance.globalTargetPlayer;
        waves[waveNumber].spawnedEnemyCount++;
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
}
