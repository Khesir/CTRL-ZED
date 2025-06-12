using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AntiVirus
{
    public string title;
    public int price;
    public AntiVirusBuff buffs;
}

[System.Serializable]
public class AntiVirusBuff
{
    public float enemySlowRate;
    public float hpRegenPerSecond;
    public float dex;
}
