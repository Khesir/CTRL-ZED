using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService
{
    private PlayerData data;
    public float currentHealth;
    public event Action onDamage;
    public PlayerService(PlayerData data)
    {
        this.data = data;
        currentHealth = this.data.osHealth;
    }
    // public PlayerData GetData()
    // {
    //     return _data;
    // }
    // public bool HasCharacter(CharacterData instance) =>
    //     _data.ownedCharacters.Contains(instance);
    public int GetPlayerCoins()
    {
        return data.coins;
    }
    public bool SpendCoins(int amount)
    {
        if (amount <= data.coins)
        {
            data.coins -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetMaxHealth()
    {
        return data.osHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        onDamage?.Invoke();
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public bool isDead()
    {
        return currentHealth <= 0;
    }
}

public class PurchaseResult
{
    public bool Success { get; }
    public string Message { get; }

    public PurchaseResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}