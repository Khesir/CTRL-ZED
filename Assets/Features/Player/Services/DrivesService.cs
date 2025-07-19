using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivesService : IDrivesService
{
    private PlayerData data;
    private DrivesChargePerResource DrivesPerCharge = new();
    public event Action OnSpendDrives;
    public DrivesService(PlayerData data)
    {
        this.data = data;
    }
    public void AddDrives(int val)
    {
        data.usableDrives += val;
    }

    public int GetDrives()
    {
        return data.usableDrives;
    }
    public bool SpendDrives(int val)
    {
        // If Drives charges are 0 then add charges and reduce bio chips
        // Then if Drives charges have charge reduce it
        if (data.usableDrives < val) return false;
        // return true if both are done, otherwise false
        // Insert validation and interaction with resources here;
        data.chargedDrives += val;
        OnSpendDrives?.Invoke();
        return true;
    }

    public int GetChargedDrives() => data.chargedDrives;
    public bool SpendRemainingCharge(int val = 1)
    {
        if (data.chargedDrives < val) return false;
        data.chargedDrives -= val;
        return true;
    }
    public DrivesChargePerResource GetResourceChargePerDrives() => DrivesPerCharge;
}
