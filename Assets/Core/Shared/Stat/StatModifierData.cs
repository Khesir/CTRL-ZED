using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TargetStat
{
    Enemy,
    Follower,
    Player,
}
[System.Serializable]
public struct StatModifierData
{
    public string statId;
    public TargetStat targetStat;
    public float value;
    public ModifierType type;
    public int priority;
}
