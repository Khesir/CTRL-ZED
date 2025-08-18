using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterService : IStatHandler
{
    private readonly CharacterData _data;
    private readonly List<IStatProvider> statProviders = new();
    public event Action onLevelUp;
    public event Action onStatChange;
    // // (Optional) dictionary if you want expandable stat system
    // private Dictionary<int, float> baseStats = new Dictionary<int, float>();

    public CharacterService(CharacterData character)
    {
        _data = character;
    }
    #region Core
    // Core Info
    public string GetName() => _data.name;
    public int GetLevel() => _data.currentLevel;
    public string GetID() => _data.id;
    public CharacterConfig GetInstance() => _data.baseData;
    public CharacterData GetData() => _data;
    // modifier effects not for gameplay effect
    public void AddStatProvider(IStatProvider provider)
    {
        statProviders.Add(provider);
        onStatChange?.Invoke();
    }

    public void RemoveStatProvider(IStatProvider provider)
    {
        statProviders.Remove(provider);
        onStatChange?.Invoke();
    }
    #endregion

    #region Experience
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
    #endregion

    #region Stats Calculation
    // Attach / Remove Buffs
    private float ApplyModifiers(string statId, float basevalue)
    {
        var modifiers = new List<StatModifier>();
        foreach (var provider in statProviders)
        {
            modifiers.AddRange(provider.GetModifiers().Where(m => m.statId == statId));
        }
        modifiers.Sort((a, b) => a.priority.CompareTo(b.priority));

        float flat = 0;
        float percentAdd = 0;
        float percentMult = 1f;

        foreach (var mod in modifiers)
        {
            switch (mod.type)
            {
                case ModifierType.Flat: flat += mod.value; break;
                case ModifierType.PercentAdd: percentAdd += mod.value; break;
                case ModifierType.PercentMult: percentMult *= 1 + mod.value; break;
            }
        }
        return (basevalue + flat) * (1 + percentAdd) * percentMult;
    }

    // Derived Stats
    public int GetAttack() => Mathf.RoundToInt(ApplyModifiers("ATK", _data.baseData.baseAttack + (_data.currentLevel * 2)));
    public int GetDefense() => Mathf.RoundToInt(ApplyModifiers("DEF", _data.baseData.defense + (_data.currentLevel * 1.5f)));
    public int GetDexterity() => Mathf.RoundToInt(ApplyModifiers("DEX", _data.baseData.dex));
    public int GetMaxHealth() => Mathf.RoundToInt(ApplyModifiers("HP", _data.baseData.baseHealth + (_data.currentLevel * 10)));
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
    public float GetStat(string id)
    {
        switch (id)
        {
            case "ATK":
                return GetAttack();
            case "DEF":
                return GetDefense();
            case "DEX":
                return GetDexterity();
            case "HP":
                return GetMaxHealth();
        }
        return -1;
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
    #endregion

    #region  Team Management
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

    #endregion

}