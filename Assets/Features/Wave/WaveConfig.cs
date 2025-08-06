using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveConfig
{
  public List<EnemyConfig> enemyConfigs;
  public float spawntTimer;
  public float spawnInterval;
  public float attackTimer;
  public float minAttackTimerDamage;
  public float maxAttackTimerDamage;
  public int requiredKills;
  public List<LootDropData> waveRewards;
}
