using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
  public List<GameObject> enemyPrefabs;
  public float spawntTimer;
  public float spawnInterval;
  public float attackTimer;
  public float minAttackTimerDamage;
  public float maxAttackTimerDamage;
  public int requiredKills;
  public Loots waveRewards;
}
