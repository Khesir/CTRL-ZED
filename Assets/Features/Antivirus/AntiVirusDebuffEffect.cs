using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AntivirusDebuffEffect", menuName = "Status Effects/Antivirus/Enemy Debuff")]
public class AntiVirusDebuffEffect : StatusEffect, IStatProvider
{
    [Header("Basic Info")]
    public string buffID = "antivirus_id_000";
    public string effectName = "Basic";
    public string description = "-5% enemy speed, +5 HP/sec, +2 Dex";
    public int cost = 500;
    public float duration = 10f;
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
