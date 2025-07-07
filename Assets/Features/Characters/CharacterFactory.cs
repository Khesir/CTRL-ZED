using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterFactory
{
    public static CharacterService CreateFromShop(CharacterConfig config)
    {
        var data = new CharacterData
        {
            id = CharacterUtils.GenerateCharID(),
            baseData = config,
            name = config.name,
            level = 1,
            currentLevel = 1,
            experience = 0,
            maxHealth = config.baseHealth,
            assignedTeam = new List<int>()
        };

        return new CharacterService(data);
    }
    // We can add test characters in case
    // We can add tutorial characters
}
