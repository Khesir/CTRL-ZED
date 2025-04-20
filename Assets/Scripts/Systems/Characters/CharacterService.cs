using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterService : MonoBehaviour
{
    private CharacterData _instance;
    public CharacterData GetInstance()
    {
        return _instance;
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
}
