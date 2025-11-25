using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffectInstance
{
    public StatusEffectData data;
    public List<GameObject> targets = new List<GameObject>();
    public float remainingTime;

    public StatusEffectInstance(StatusEffectData data)
    {
        this.data = data;
        remainingTime = data.duration;
    }

    public void ApplyToTarget(GameObject target)
    {
        if (target == null || targets.Contains(target)) return;

        foreach (var effect in data.effects)
        {
            effect.Apply(target);
        }
        targets.Add(target);
    }

    public void RemoveFromTarget(GameObject target)
    {
        if (target == null || !targets.Contains(target)) return;

        foreach (var effect in data.effects)
        {
            effect.Remove(target);
        }
        targets.Remove(target);
    }

    public void RemoveFromAll()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] != null)
            {
                foreach (var effect in data.effects)
                {
                    effect.Remove(targets[i]);
                }
            }
        }
        targets.Clear();
    }

    public void CleanupDestroyedTargets()
    {
        targets.RemoveAll(t => t == null);
    }

    public bool Tick(float deltaTime)
    {
        CleanupDestroyedTargets();

        if (data.duration <= 0f) return false; // Permanent buff
        remainingTime -= deltaTime;
        if (remainingTime <= 0f)
        {
            RemoveFromAll();
            return true;
        }
        return false;
    }
}
