using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int coins;
    public int maxTeam;
    public PlayerData(int coins = 6000, int maxTeam = 2)
    {
        this.coins = coins;
        this.maxTeam = maxTeam;
    }
}
