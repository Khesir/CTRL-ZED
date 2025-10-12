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
        // I know this is bad in name sense
        // Just keep event same with spending drive
        OnSpendDrives?.Invoke();
    }

    public int GetDrives()
    {
        return data.usableDrives;
    }
    public bool SpendDrives(int val)
    {
        data.chargedDrives += val;
        data.usableDrives -= val;
        OnSpendDrives?.Invoke();
        return true;
    }
    public bool SpendChargeDrives(int val)
    {
        if (data.chargedDrives < val) return false;
        data.chargedDrives -= val;
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
