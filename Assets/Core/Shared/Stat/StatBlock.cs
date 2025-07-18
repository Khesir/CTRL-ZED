using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatBlock : MonoBehaviour
{
    private readonly List<IStatProvider> providers = new();

    public void AddProvider(IStatProvider provider) => providers.Add(provider);
    public void RemoveProvider(IStatProvider provider) => providers.Remove(provider);

    public float GetStat(string statId)
    {
        var allMods = providers
            .SelectMany(p => p.GetModifiers())
            .Where(m => m.statId == statId)
            .OrderBy(m => m.priority)
            .ToList();

        float baseValue = 0f;
        float percentAdd = 0f;
        float percentMult = 1f;

        foreach (var mod in allMods)
        {
            switch (mod.type)
            {
                case ModifierType.Flat:
                    baseValue += mod.value;
                    break;
                case ModifierType.PercentAdd:
                    percentAdd += mod.value;
                    break;
                case ModifierType.PercentMult:
                    percentMult *= 1 + mod.value;
                    break;
            }
        }

        float finalValue = (baseValue + (baseValue * percentAdd)) * percentMult;
        return finalValue;
    }
    // private Dictionary<StatType, float> baseStats = new();
    // private Dictionary<StatType, List<StatModifier>> modifiers = new();

    // public void SetBaseStat(StatType type, float value) => baseStats[type] = value;

    // public void AddModifier(StatType type, StatModifier mod)
    // {
    //     if (!modifiers.ContainsKey(type))
    //         modifiers[type] = new List<StatModifier>();
    //     modifiers[type].Add(mod);
    // }
    // public void RemoveAllModifiersFrom(object source)
    // {
    //     foreach (var mods in modifiers.Values)
    //         mods.RemoveAll(m => m.source == source);
    // }
    // public float GetStat(StatType type)
    // {
    //     if (!baseStats.TryGetValue(type, out var baseValue)) baseValue = 0;
    //     if (!modifiers.TryGetValue(type, out var mods)) return baseValue;

    //     float flat = 0f, percentAdd = 0f, percentMult = 1f;

    //     foreach (var mod in mods)
    //     {
    //         switch (mod.type)
    //         {
    //             case ModifierType.Flat: flat += mod.value; break;
    //             case ModifierType.PercentAdd: percentAdd += mod.value; break;
    //             case ModifierType.PercentMult: percentMult += mod.value; break;
    //         }
    //     }
    //     return (baseValue + flat) * (1 + percentAdd) * percentAdd;
    // }
}
