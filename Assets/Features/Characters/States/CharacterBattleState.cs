using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBattleState
{
    public CharacterService data;
    public float currentHealth;
    public CharacterBattleState(CharacterService service)
    {
        data = service;
        currentHealth = data.GetMaxHealth();
    }
    public bool isDead => currentHealth <= 0;
    public bool TakeDamage(float dmg)
    {
        currentHealth = Mathf.Max(currentHealth - dmg, 0f);
        data.InvokeOnDamage();
        return isDead;
    }
}
