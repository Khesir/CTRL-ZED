using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public ResourceService service;
    public event Action onCoinsChange;
    public async UniTask Initialize(ResourceData data)
    {
        service = new ResourceService(data);
        await UniTask.CompletedTask;
    }
    public int GetFood()
    {
        return service.GetFood();
    }
    public int GetTechnology()
    {
        return service.GetTechnology();
    }
    public int GetEnergy()
    {
        return service.GetEnergy();
    }
    public int GetIntelligence()
    {
        return service.GetIntelligence();
    }
    public int GetCoins()
    {
        return service.GetCoins();
    }
    public void SpendFood(int val)
    {
        service.SpendFood(val);
    }
    public void SpendTechnology(int val)
    {
        service.SpendTechnology(val);
    }
    public void SpendEnergy(int val)
    {
        service.SpendEnergy(val);
    }
    public void SpendIntelligence(int val)
    {
        service.SpendIntelligence(val);
    }
    public bool SpendCoins(int val)
    {
        var res = service.SpendCoins(val);
        onCoinsChange?.Invoke();
        return res;
    }
    public void AddFood(int val)
    {
        service.AddFood(val);
    }
    public void AddTechnology(int val)
    {
        service.AddTechnology(val);
    }
    public void AddEnergy(int val)
    {
        service.AddEnergy(val);
    }
    public void AddIntelligence(int val)
    {
        service.AddIntelligence(val);
    }
    public void AddCoins(int val)
    {
        service.AddCoins(val);
    }
}
