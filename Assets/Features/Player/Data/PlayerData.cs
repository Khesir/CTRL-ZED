using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerData
{
    [Header("Player Progression")]
    public bool completedTutorial = false;
    // Configurable properties
    [Header("Configurable Properties")]
    public int maxTeam = 2;
    public int maxLevel = 10;
    public float coinsPerExp = 2f;
    public float healthPerCoin;

    // Progression state
    [Header("Progress")]
    public int level = 1;
    public int currentExp = 0;
    public float currentHealth = -1;

    // Level configuration (health/exp curve)
    // Go to LevelingSystem, find the oscurve
    [Header("Resources")]
    public ResourceData resources = new();
    // Bio chips implementation
    [Header("Drives")]
    public int chargedDrives = 0;
    public int usableDrives = 50;
    // Economy
    [Header("Economy")]
    public int coins = 50000;
}
