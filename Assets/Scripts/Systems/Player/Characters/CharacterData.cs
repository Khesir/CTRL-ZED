using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CharacterData
{
    public string id;
    public CharacterConfig baseData;

    public float maxHealth;
    public string name;
    public int level;


    public int maxLevel;
    public int currentLevel;
    public int experience;
    public List<int> playerLevels = new();
    public List<int> assignedTeam = new();

    public CharacterData(CharacterConfig templateData)
    {
        id = Guid.NewGuid().ToString();
        baseData = templateData;
        name = NameGenerator.GetRandomName();
        level = templateData.level;
        maxHealth = baseData.baseHealth * level;
        maxLevel = 90;
        GenerateLevel();
    }
    public void GenerateLevel()
    {

        int baseXP = 50;
        playerLevels.Add(baseXP);

        for (int i = 1; i < maxLevel; i++)
        {
            int nextXP = Mathf.CeilToInt(playerLevels[i - 1] * 1.1f);
            playerLevels.Add(nextXP);
        }
    }
    // This is optional and situational, currently being used on StatsUI
    public Dictionary<string, int> GetStatMap()
    {
        return new Dictionary<string, int>
        {
            { "ATK", baseData.baseAttack + (level * 2) },
            { "DEF", baseData.defense + Mathf.FloorToInt(level * 1.5f) },
            { "DEX", baseData.dex },
            { "HP", baseData.baseHealth + (level * 10) }
        };
    }
    public Dictionary<string, float> GetDeploymentCost()
    {
        float multiplier = Mathf.Pow(1.2f, level - 1);
        return new Dictionary<string, float>{
            {"Food", baseData.food * multiplier },
            {"Technology", baseData.technology * multiplier},
            {"Energy", baseData.energy * multiplier},
            {"Intelligence", baseData.intelligence* multiplier}
        };
    }
}
