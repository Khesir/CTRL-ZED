using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier
{
    public string statId;
    public float value;
    public ModifierType type;
    public int priority;
    public object source;

    public StatModifier(string statId, float value, ModifierType type, int priority = 0, object source = null)
    {
        this.statId = statId;
        this.value = value;
        this.type = type;
        this.priority = priority;
        this.source = source;
    }
}
public enum ModifierType
{
    Flat,          // +10
    PercentAdd,    // +10%
    PercentMult    // x1.10
}