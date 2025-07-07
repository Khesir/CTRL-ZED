using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService
{
    private PlayerData data;

    public event Action onDamage;
    public event Action onGainExp;

    public PlayerService(PlayerData data)
    {
        this.data = data;
    }

    public void Heal()
    {
        data.currentHealth = data.osLevel[data.level].osHealth;
        onDamage?.Invoke();
    }

    public float GetHealthPerCoin()
    {
        return data.healthPerCoin;
    }

    public float GetMaxHealth()
    {
        return data.osLevel[data.level].osHealth;
    }

    public int GetMaxExp()
    {
        return data.osLevel[data.level].requiredExp;
    }

    public float GetCoinsPerExpRate()
    {
        return data.coinsPerExp;
    }

    public void TakeDamage(float damage)
    {
        data.currentHealth -= damage;
        onDamage?.Invoke();
    }

    public bool isDead()
    {
        return data.currentHealth <= 0;
    }

    public float GetCurrentHealth()
    {
        return data.currentHealth;
    }

    public int GetCurrentExp()
    {
        return data.currentExp;
    }

    public int GetLevel()
    {
        return data.level;
    }

    public int GetMaxLevel()
    {
        return data.maxLevel;
    }

    public int GetRequiredCoinsToLevelup()
    {
        return data.osLevel[data.level].requiredCoins;
    }


    public void GainExp(int currentExp)
    {
        data.currentExp += currentExp;
        if (data.currentExp >= data.osLevel[data.level].requiredExp)
        {
            data.level++;
            data.currentExp = 0;
            data.currentHealth = data.osLevel[data.level].osHealth;
        }
        onGainExp?.Invoke();
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