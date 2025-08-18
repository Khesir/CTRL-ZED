using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData playerData;
    public List<CharacterData> ownedCharacters;
    public List<Team> teams;
    public int antiVirusLevel;
    public SaveData()
    {
        playerData = new PlayerData { };
        ownedCharacters = new List<CharacterData>();
        teams = new List<Team>();
        // resourceData = new ResourceData(coins: 100000);
        antiVirusLevel = 0;
    }
}
