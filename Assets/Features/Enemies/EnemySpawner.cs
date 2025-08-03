using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

    public Transform minPos;
    public Transform maxPos;
    public GameObject prefab;
    public void SpawnEnemy(EnemyConfig enemyConfig)
    {
        Vector2 spawnPoint = GetRandomSpawnPoint();
        var go = Instantiate(prefab, spawnPoint, Quaternion.identity);
        go.GetComponent<EnemyService>().Initialize(enemyConfig);
    }

    public Vector2 GetRandomSpawnPoint()
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
