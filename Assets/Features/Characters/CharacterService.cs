using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterService
{
    private readonly CharacterData _data;
    public event Action onDamage;
    public event Action onLevelUp;
    public event Action onStatChange;
    public CharacterService(CharacterData character)
    {
        _data = character;
    }
    // Core Info
    public string GetName() => _data.name;
    public int GetLevel() => _data.currentLevel;
    public string GetID() => _data.id;
    public CharacterConfig GetInstance() => _data.baseData;
    // Progression
    public void GetExperience(int amount)
    {
        _data.experience += amount;

        while (_data.experience >= GetRequiredExp())
        {
            _data.experience -= GetRequiredExp();
            _data.currentLevel = Mathf.Min(_data.currentLevel + 1, GetMaxLevel());
            onLevelUp?.Invoke();
        }

        onStatChange?.Invoke();
    }
    public int GetMaxLevel() => LevelingSystem.CharacterCurve.MaxLevel;
    public int GetRequiredExp() => LevelingSystem.CharacterCurve.GetRequiredExp(_data.currentLevel);

    // Derived Stats
    public int GetAttack() => _data.baseData.baseAttack + (_data.currentLevel * 2);
    public int GetDefense() => _data.baseData.defense + Mathf.FloorToInt(_data.currentLevel * 1.5f);
    public int GetDexterity() => _data.baseData.dex;
    public int GetMaxHealth() => _data.baseData.baseHealth + (_data.currentLevel * 10);

    public Dictionary<string, int> GetStatMap()
    {
        return new()
        {
            { "ATK", GetAttack() },
            { "DEF", GetDefense() },
            { "DEX", GetDexterity() },
            { "HP", GetMaxHealth() }
        };
    }
    public Dictionary<string, float> GetDeploymentCost()
    {
        // Deployment cost multipler change
        float multiplier = Mathf.Pow(1.2f, _data.currentLevel - 1);
        return new Dictionary<string, float>{
            {"Food", _data.baseData.food * multiplier },
            {"Technology", _data.baseData.technology * multiplier},
            {"Energy", _data.baseData.energy * multiplier},
            {"Intelligence", _data.baseData.intelligence* multiplier}
        };
    }
    // Team Assignment
    public Response<object> AssigntoTeam(int teamIndex)
    {
        if (!_data.assignedTeam.Contains(teamIndex))
        {
            _data.assignedTeam.Add(teamIndex);
            return Response.Success("Character assigned successfully.");
        }
        else
        {
            return Response.Fail("Character already in this team.");

        }
    }

    public Response<object> RemoveFromTeam(int teamIndex)
    {
        _data.assignedTeam.Remove(teamIndex);
        return Response.Success($"Successfully Removed {teamIndex}");
    }
    public bool isInTeam(int teamIndex)
    {
        return _data.assignedTeam.Contains(teamIndex);
    }

}