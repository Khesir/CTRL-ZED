using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBattleState : MonoBehaviour
{
    public CharacterService characterService;
    public float currentHealth;
    public CharacterBattleState(CharacterService service)
    {
        characterService = service;
    }
    public bool isDead => currentHealth <= 0;
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        characterService.InvokeOnDamage();
    }
}
