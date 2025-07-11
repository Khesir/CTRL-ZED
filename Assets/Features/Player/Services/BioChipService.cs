using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioChipService : IBioChipService
{
    private PlayerData data;
    private int biochipPerCharge;
    public event Action OnSpendBioChip;
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
        // If biochip charges are 0 then add charges and reduce bio chips
        // Then if biochip charges have charge reduce it
        if (data.biochips < val) return false;
        // return true if both are done, otherwise false
        data.bioChipsRemainingCharges += biochipPerCharge;
        OnSpendBioChip?.Invoke();
        return true;
    }

    public int GetRemainingCharge() => data.bioChipsRemainingCharges;
    public bool SpendRemainingCharge(int val = 1)
    {
        if (data.bioChipsRemainingCharges < val) return false;
        data.bioChipsRemainingCharges -= val;
        return true;
    }
}
