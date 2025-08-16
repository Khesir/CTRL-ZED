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
public enum BuffType
{
    AntiVirus
}
public enum BuffDurationType
{
    Temporary,
    Permanent,
    PerLevel,
}
public enum EffectScope
{
    GameplayOnly,  // Player, Enemy, etc.
    MetaOnly,      // Main menu, shop discounts, account boosts
    Both
}
[CreateAssetMenu(fileName = "NewBuff", menuName = "Buffs/BuffData")]
public class StatusEffectData : ScriptableObject
{
    public int id;
    public Sprite icon;
    public string title;
    public string description = "-5% enemy speed, +5 HP/sec, +2 Dex";
    public int price;
    public BuffSourceType sourceType;
    public BuffDurationType durationType;
    public float duration; // If its purchasable
    public List<StatusEffect> effects;
    public EffectScope effectScope = EffectScope.GameplayOnly;
    public BuffType buffType;
}
