using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterFactory
{
    public static CharacterData CreateFromShop(CharacterConfig config)
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

        return data;
    }
    public static CharacterData CreateTestCharacter()
    {
        List<CharacterConfig> possibleConfigs = ServiceLocator.Get<ICharacterManager>().characterTemplates;
        if (possibleConfigs == null || possibleConfigs.Count == 0)
        {
            Debug.LogError("No character configs available.");
            return null;
        }

        var config = possibleConfigs[UnityEngine.Random.Range(0, possibleConfigs.Count)];

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

        return data;
    }
    // We can add test characters in case
    // We can add tutorial characters
}
