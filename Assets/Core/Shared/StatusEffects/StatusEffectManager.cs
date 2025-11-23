using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour, IStatusEffectManager
{
    public List<StatusEffectInstance> activeBuffs { get; private set; }
    public event Action onBuffChange;
    public async UniTask Initialize()
    {
        activeBuffs = new();
        await UniTask.CompletedTask;
    }
    public void AddBuff(StatusEffectData data)
    {
        // Currently lacking a target to apply
        StatusEffectInstance instance = new StatusEffectInstance(data);
        activeBuffs.Add(instance);
        onBuffChange?.Invoke();
    }
    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            bool isExpired = activeBuffs[i].Tick(dt);
            if (isExpired)
            {
                activeBuffs.RemoveAt(i);
            }
        }
        onBuffChange?.Invoke();
    }
    public void ClearAllBuffs()
    {
        foreach (var buff in activeBuffs)
        {
            buff.Remove();
        }
        activeBuffs.Clear();
        onBuffChange.Invoke();
    }
    public List<StatusEffectInstance> GetAllStatusInstances()
    {
        return activeBuffs;
    }
    public bool IsStatusEffectAdded(StatusEffectData data)
    {
        foreach (var instance in activeBuffs)
        {
            if (instance.data.id == data.id)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsThereExisitingBuffType(StatusEffectData data)
    {
        foreach (var instance in activeBuffs)
        {
            if (instance.data.buffType == data.buffType)
            {
                return true;
            }
        }
        return false;
    }
}
