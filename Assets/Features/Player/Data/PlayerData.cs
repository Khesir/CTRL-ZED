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
    public float healthPerCoin = 5f;
    public float coinsPerExp = 2f;

    // Progression state
    public int level = 1;
    public int currentExp = 0;
    public float currentHealth = 0f;

    // Level configuration (health/exp curve)
    // Go to LevelingSystem, find the oscurve
}
