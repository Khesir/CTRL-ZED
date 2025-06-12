using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public PlayerData playerData;
    public ResourceData resourceData;
    public List<CharacterData> ownedCharacters;
    public List<Team> teams;
    public int antiVirusLevel;
    public SaveData()
    {
        playerData = new PlayerData(maxTeam: 3);
        ownedCharacters = new List<CharacterData>();
        teams = new List<Team>();
        resourceData = new ResourceData(coins: 100000);
        antiVirusLevel = 0;
    }
}
