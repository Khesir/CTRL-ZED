using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public int coins;
    public int maxTeam;
    public float osHealth;
    public PlayerData(int coins = 6000, int maxTeam = 2, float osHealth = 1000)
    {
        this.coins = coins;
        this.maxTeam = maxTeam;
        this.osHealth = osHealth;
    }
}
