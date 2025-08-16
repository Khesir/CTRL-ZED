using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AntivirusDebuffEffect", menuName = "Status Effects/Antivirus/Enemy Debuff")]
public class AntiVirusDebuffEffect : StatusEffect, IStatProvider
{
    public override TargetType Target => TargetType.AllEnemies;

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
        var service = target.GetComponent<EnemyService>();
        if (service != null)
            service.AddStatProvider(this);
    }

    public override void Remove(GameObject target)
    {
        var service = target.GetComponent<EnemyService>();
        if (service != null)
            service.RemoveStatProvider(this);
    }
}
