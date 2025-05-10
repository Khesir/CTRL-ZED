using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData
{

    public int food;
    public int technology;
    public int energy;
    public int intelligence;
    public int coins;
    public int bioChips;
    public int maxBioChipCharges = 5;
    public ResourceData(int food = 1000, int technology = 1000, int energy = 1000, int intelligence = 1000, int coins = 1000, int bioChips = 1)
    {
        this.food = food;
        this.technology = technology;
        this.energy = energy;
        this.intelligence = intelligence;
        this.coins = coins;
        this.bioChips = bioChips;
    }
}

