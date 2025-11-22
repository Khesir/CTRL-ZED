using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyManager
{
    int ActiveEnemyCount { get; }
    bool InStealth { get; set; }

    void Initialize();
    void RegisterEnemy(EnemyService enemy);
    void UnregisterEnemy(EnemyService enemy);
    void KillAllEnemies(bool silent = false);
    void SetStealth(bool val);
    void ResetTargets();
}
