using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBattleState
{
    public CharacterService data;
    public float currentHealth;
    public event Action onDamage;
    public event Action onHeal;
    public event Action onDeath;
    public bool isDead = false;
    public CharacterBattleState(CharacterService service)
    {
        data = service;
        currentHealth = data.GetMaxHealth();
    }
    public void TakeDamage(float dmg)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(currentHealth - dmg, 0f);
        onDamage?.Invoke();
        if (currentHealth <= 0f)
        {
            isDead = true;
            onDeath?.Invoke();
            Debug.Log("Character Dead!");
        }
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
}
