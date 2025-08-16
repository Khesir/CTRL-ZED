using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffectInstance
{
    public StatusEffectData data;
    public GameObject target;
    public float remainingTime;

    public StatusEffectInstance(StatusEffectData data)
    {
        this.data = data;
        remainingTime = data.duration;
    }
    public void Apply()
    {
        foreach (var effect in data.effects)
        {
            effect.Apply(target);
        }
    }
    public void Remove()
    {
        foreach (var effect in data.effects)
        {
            effect.Remove(target);
        }
    }
    public bool Tick(float deltaTime)
    {
        if (data.duration <= 0f) return false; // Perpament buff
        remainingTime -= deltaTime;
        if (remainingTime <= 0f)
        {
            Remove();
            return true;
        }
        return false;
    }
}
