using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceService : IResourceService
{
    private ResourceData data;
    public event Action OnResourceChange;
    public ResourceService(PlayerData data)
    {
        this.data = data.resources;
    }

    // Food
    public int GetFood()
    {
        return data.food;
    }
    public void SpendFood(int val)
    {
        if (val <= data.food)
        {
            data.food -= val;
            OnResourceChange?.Invoke();
        }
    }
    public void AddFood(int val)
    {
        data.food += val;
        OnResourceChange?.Invoke();
    }

    // Technology
    public int GetTechnology()
    {
        return data.technology;
    }
    public void SpendTechnology(int val)
    {
        if (val <= data.technology)
        {
            data.technology -= val;
            OnResourceChange?.Invoke();
        }
    }
    public void AddTechnology(int val)
    {
        data.technology += val;
        OnResourceChange?.Invoke();
    }

    // Energy
    public int GetEnergy()
    {
        return data.energy;
    }
    public void SpendEnergy(int val)
    {
        if (val <= data.energy)
        {
            data.energy -= val;
            OnResourceChange?.Invoke();
        }
    }
    public void AddEnergy(int val)
    {
        data.energy += val;
        OnResourceChange?.Invoke();
    }

    // Intelligence
    public int GetIntelligence()
    {
        return data.intelligence;
    }
    public void SpendIntelligence(int val)
    {
        if (val <= data.intelligence)
        {
            data.intelligence -= val;
            OnResourceChange?.Invoke();
        }
    }
    public void AddIntelligence(int val)
    {
        data.intelligence += val;
        OnResourceChange?.Invoke();
    }
}
