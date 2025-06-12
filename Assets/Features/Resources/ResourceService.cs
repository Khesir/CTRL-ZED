using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceService
{
    private ResourceData _data;
    public int bioChipsRemainingCharges;
    public ResourceService(ResourceData data)
    {
        if (data != null)
        {
            _data = data;
        }
        else
        {
            _data = new ResourceData();
        }
        bioChipsRemainingCharges = 0;
    }
    public int GetBioChips()
    {
        return _data.bioChips;
    }
    public void SpendBioChips()
    {
        if (_data.bioChips > 0)
        {
            _data.bioChips--;
            bioChipsRemainingCharges += _data.maxBioChipCharges;
        }
    }
    public int GetFood()
    {
        return _data.food;
    }
    public int GetTechnology()
    {
        return _data.technology;
    }
    public int GetEnergy()
    {
        return _data.energy;
    }
    public int GetIntelligence()
    {
        return _data.intelligence;
    }
    public int GetCoins()
    {
        return _data.coins;
    }
    public void SpendFood(int val)
    {
        if (val <= _data.food)
        {
            _data.food -= val;
        }
    }
    public void SpendTechnology(int val)
    {
        if (val <= _data.technology)
        {
            _data.technology -= val;
        }
    }
    public void SpendEnergy(int val)
    {
        if (val <= _data.energy)
        {
            _data.energy -= val;
        }
    }
    public void SpendIntelligence(int val)
    {
        if (val <= _data.intelligence)
        {
            _data.intelligence -= val;
        }
    }
    public bool SpendCoins(int val)
    {
        if (val <= _data.coins)
        {
            _data.coins -= val;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddFood(int val)
    {
        _data.food += val;
    }
    public void AddTechnology(int val)
    {
        _data.technology += val;
    }
    public void AddEnergy(int val)
    {
        _data.energy += val;
    }
    public void AddIntelligence(int val)
    {
        _data.intelligence += val;
    }
    public void AddCoins(int val)
    {
        _data.coins += val;
    }

}
