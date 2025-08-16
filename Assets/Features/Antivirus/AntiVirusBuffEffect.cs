using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AntivirusBuffEffect", menuName = "Status Effects/Antivirus/Player Buff")]
public class AntiVirusBuffEffect : StatusEffect, IStatProvider
{
    public override TargetType Target => TargetType.AllPlayers;

    [Header("Stat Modifiers")]
    [SerializeField] private List<StatModifierData> modifiers;

    public IEnumerable<StatModifier> GetModifiers()
    {
        foreach (var mod in modifiers)
        {
            if (mod.value != 0)
            {
                yield return new StatModifier(mod.statId, mod.value, mod.type, mod.priority, this);
            }
        }
    }

    public override void Apply(GameObject target)
    {
        var service = target.GetComponent<CharacterService>();
        if (service != null)
            service.AddStatProvider(this);
    }

    public override void Remove(GameObject target)
    {
        var service = target.GetComponent<CharacterService>();
        if (service != null)
            service.RemoveStatProvider(this);
    }
}
