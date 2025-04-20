using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int coins;
    public List<CharacterData> ownedCharacters;
    public int currentMaxTeam;
    public List<Team> teams = new();
    public PlayerData()
    {
        coins = 1000;
        ownedCharacters = new List<CharacterData>();

        currentMaxTeam = 2;
    }
}
