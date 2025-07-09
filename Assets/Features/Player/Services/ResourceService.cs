using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceService : IResourceService
{
    private ResourceData data;
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
        }
    }
    public void AddFood(int val)
    {
        data.food += val;
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
        }
    }
    public void AddTechnology(int val)
    {
        data.technology += val;
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
        }
    }
    public void AddEnergy(int val)
    {
        data.energy += val;
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
        }
    }
    public void AddIntelligence(int val)
    {
        data.intelligence += val;
    }
}
