using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private readonly List<EnemyService> activeEnemies = new();

    public void RegisterEnemy(EnemyService enemy)
    {
        if (!activeEnemies.Contains(enemy)) activeEnemies.Add(enemy);
    }
    public void UnregisterEnemy(EnemyService enemy)
    {
        activeEnemies.Remove(enemy);
    }
    public void KillAllEnemies(bool silent = false)
    {
        foreach (var enemy in new List<EnemyService>(activeEnemies))
        {
            if (enemy == null) continue;
            if (silent)
                enemy.SilentKill();
            else
                enemy.TakeDamage(float.MaxValue);
        }
        activeEnemies.Clear();
    }

    public int ActiveEnemyCount => activeEnemies.Count;
}
