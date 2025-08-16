using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TargetType
{
    Player,
    Enemies,
    Follower,
    AllEnemies,
    AllPlayers,
}
public abstract class StatusEffect : ScriptableObject
{
    public abstract TargetType Target { get; }

    public abstract void Apply(GameObject target);
    public abstract void Remove(GameObject target);
}