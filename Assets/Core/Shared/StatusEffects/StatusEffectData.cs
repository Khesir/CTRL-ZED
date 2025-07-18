using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffSourceType
{
    Purchasable,
    Skill,
    EnemyCast,
    Passive
}

public enum BuffDurationType
{
    Temporary,
    Permanent
}
[CreateAssetMenu(fileName = "NewBuff", menuName = "Buffs/BuffData")]
public class StatusEffectData : ScriptableObject
{
    public string title;
    public int price;
    public BuffSourceType sourceType;
    public BuffDurationType durationType;
    public float duration; // If its purchasable
    public List<StatusEffect> effects;
}
