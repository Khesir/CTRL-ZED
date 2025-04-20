using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstance
{
    public CharacterData template;

    public int currentHealth;
    public string name;
    public int level = 1;

    public CharacterInstance(CharacterData templateData)
    {
        template = templateData;
        name = NameGenerator.GetRandomName();
        currentHealth = template.baseHealth;
        level = 1;
    }
    public int GetAttack()
    {
        return template.baseAttack + (level * 2);
    }

    public int GetDefense()
    {
        return template.defense + Mathf.FloorToInt(level * 1.5f);
    }

    public int GetSpeed()
    {
        return template.dex;
    }

    public int GetMaxHealth()
    {
        return template.baseHealth + (level * 10);
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
