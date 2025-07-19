using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerData
{
    // Configurable properties
    public int maxTeam = 2;
    public int maxLevel = 10;
    public float coinsPerExp = 2f;
    public float healthPerCoin;

    // Progression state
    public int level = 1;
    public int currentExp = 0;
    public float currentHealth = -1;

    // Level configuration (health/exp curve)
    // Go to LevelingSystem, find the oscurve
    public ResourceData resources = new();
    // Bio chips implementation
    public int chargedDrives = 0;
    public int usableDrives = 50;
    // Economy
    public int coins = 50000;
}
