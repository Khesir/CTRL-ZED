using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBattleState : IStatHandler, IDamageable
{
    public CharacterService data;
    public float currentHealth;
    public event Action onDamage;
    public event Action onHeal;
    public event Action onDeath;

    public bool isDead => currentHealth == 0;
    // Stat providers
    public bool isDisabled = false;
    public CharacterBattleState(CharacterService service)
    {
        data = service;
        currentHealth = data.GetMaxHealth();
    }
    private readonly List<IStatProvider> statProviders = new();
    // ---- IStatHandler ----
    // 
    public void AddStatProvider(IStatProvider provider) => statProviders.Add(provider);
    public void RemoveStatProvider(IStatProvider provider) => statProviders.Remove(provider);

    public float GetStat(string statId)
    {
        float baseValue = data.GetStat(statId);

        var modifiers = new List<StatModifier>();

        foreach (var provider in statProviders)
        {
            modifiers.AddRange(provider.GetModifiers().Where(m => m.statId == statId));
        }
        modifiers.Sort((a, b) => a.priority.CompareTo(b.priority));

        float flat = 0;
        float percentAdd = 0;
        float percentMult = 1f;

        foreach (var mod in modifiers)
        {
            switch (mod.type)
            {
                case ModifierType.Flat: flat += mod.value; break;
                case ModifierType.PercentAdd: percentAdd += mod.value; break;
                case ModifierType.PercentMult: percentMult *= 1 + mod.value; break;
            }
        }
        return (baseValue + flat) * (1 + percentAdd) * percentMult;
    }

    public float Heal(float amount)
    {
        float maxHealth = data.GetMaxHealth();
        float previousHealth = currentHealth;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        float actualHealed = currentHealth - previousHealth;

        onHeal?.Invoke();
        return actualHealed;
    }

    public void TakeDamage(float dmg, GameObject source = null)
    {
        if (isDead || isDisabled)
            return;
        float defense = GetStat("DEF");
        // (guarantee at least 1 damage so super tanky units don’t become invincible)
        float reducedDamage = Mathf.Max(dmg - defense, 1f);

        currentHealth = Mathf.Max(currentHealth - reducedDamage, 0f);
        onDamage?.Invoke();
        if (currentHealth <= 0f)
        {
            onDeath?.Invoke();
            Debug.Log("Character Dead!");
        }
    }
}
