using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public PlayerData playerData;
    public List<CharacterData> ownedCharacters;
    public List<Team> teams;
    public SaveData()
    {
        playerData = new PlayerData(coins: 100000, maxTeam: 3);
        ownedCharacters = new List<CharacterData>();
        teams = new List<Team>();
    }
}
