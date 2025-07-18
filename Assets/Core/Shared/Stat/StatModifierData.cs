using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct StatModifierData
{
    public string statId;
    public float value;
    public ModifierType type;
    public int priority;
}
