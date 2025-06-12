using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int maxTeam;
    public int maxLevel = 10;
    public List<OsHealth> osLevel = new();
    public float osHealth = 1000;
    public int level;
    public int currentExp;
    public float currentHealth;
    public float coinsPerExp = 2.0f;
    public float healthPerCoin = 5f;
    public PlayerData(int maxTeam = 2, int level = 1, int currentExp = 0, float currentHealth = 0)
    {
        GenerateLevel();
        this.maxTeam = maxTeam;
        this.level = level;
        this.currentExp = currentExp;
        this.currentHealth = currentHealth == 0 ? osLevel[level].osHealth : currentHealth;
    }

    public void GenerateLevel()
    {
        osLevel.Clear();
        float currentHealth = osHealth;
        int baseExp = 50;

        for (int i = 0; i < maxLevel; i++)
        {
            osLevel.Add(new OsHealth(currentHealth, baseExp));
            currentHealth *= 1.5f;
            baseExp += (int)(baseExp * 1.5f);
        }
    }
}
[System.Serializable]
public class OsHealth
{
    public float osHealth;
    public int requiredExp;
    public int requiredCoins;

    public OsHealth(float osHealth, int requiredExp)
    {
        this.osHealth = osHealth;
        this.requiredExp = requiredExp;
    }
}