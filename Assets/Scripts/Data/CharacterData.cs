using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CharacterData
{
    public string id;
    public CharacterConfig baseData;

    public int currentHealth;
    public string name;
    public int level = 1;
    public List<int> assignedTeam = new();

    public CharacterData(CharacterConfig templateData)
    {
        id = Guid.NewGuid().ToString();
        baseData = templateData;
        name = NameGenerator.GetRandomName();
        currentHealth = baseData.baseHealth;
        level = 1;
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
}
