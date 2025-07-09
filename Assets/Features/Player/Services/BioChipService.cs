using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioChipService : IBioChipService
{
    private PlayerData data;
    private int biochipPerCharge;
    public BioChipService(PlayerData data, int biochipPerCharge = 10)
    {
        this.data = data;
        this.biochipPerCharge = biochipPerCharge;
    }

    public void AddBioChip(int val)
    {
        data.biochips += val;
    }

    public int GetBioChip()
    {
        return data.biochips;
    }

    public bool SpendBioChip(int val)
    {
        if (data.bioChipsRemainingCharges >= 0 && data.biochips >= 0)
            return false;
        data.biochips += biochipPerCharge;
        return true;
    }
}
