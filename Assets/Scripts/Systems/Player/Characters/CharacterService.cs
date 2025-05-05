using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterService
{
    private CharacterData _instance;
    public CharacterService(CharacterData character)
    {
        _instance = character;
    }
    public CharacterData GetInstance()
    {
        return _instance;
    }
    public void CreateCharacter(CharacterConfig template)
    {
        _instance = new CharacterData(template);
    }
    // Stats 
    public string GetName()
    {
        return _instance.name;
    }
    public int GetLevel()
    {
        return _instance.level;
    }
    public int GetAttack()
    {
        return _instance.baseData.baseAttack + (_instance.level * 2);
    }

    public int GetDefense()
    {
        return _instance.baseData.defense + Mathf.FloorToInt(_instance.level * 1.5f);
    }

    public int GetSpeed()
    {
        return _instance.baseData.dex;
    }

    public int GetMaxHealth()
    {
        return _instance.baseData.baseHealth + (_instance.level * 10);
    }
    public Dictionary<string, int> GetStatMap()
    {
        return new Dictionary<string, int>
        {
            { "ATK", GetAttack() },
            { "DEF", GetDefense() },
            { "DEX", GetSpeed() },
            { "HP", GetMaxHealth() }
        };
    }
    // Assigned Team
    public Response<object> AssigntoTeam(int teamIndex)
    {
        if (!_instance.assignedTeam.Contains(teamIndex))
        {
            _instance.assignedTeam.Add(teamIndex);
            return Response.Success("Character assigned successfully.");
        }
        else
        {
            return Response.Fail("Character already in this team.");

        }
    }

    public Response<object> RemoveFromTeam(int teamIndex)
    {
        _instance.assignedTeam.Remove(teamIndex);
        return Response.Success($"Successfully Removed {teamIndex}");
    }
    public bool isInTeam(int teamIndex)
    {
        return _instance.assignedTeam.Contains(teamIndex);
    }
}