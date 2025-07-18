using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private readonly List<StatusEffectInstance> activeBuffs = new();

    public void AddBuff(StatusEffectData data)
    {
        StatusEffectInstance instance = new StatusEffectInstance(data, gameObject);
        instance.Apply();
        activeBuffs.Add(instance);
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
    }
    public void ClearAllBuffs()
    {
        foreach (var buff in activeBuffs)
        {
            buff.Remove();
        }
        activeBuffs.Clear();
    }
}
