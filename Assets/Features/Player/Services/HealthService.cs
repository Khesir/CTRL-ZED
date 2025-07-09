using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthService : IHealthService
{
    private PlayerData data;
    private const float hpLevelMultiplier = 1.5f;
    private const float baseHealth = 100f;
    public event Action OnHealthChanged;

    public HealthService(PlayerData data)
    {
        this.data = data;
    }
    public void Heal()
    {
        data.currentHealth = GetMaxHealth();
    }
    public void TakeDamage(float amount)
    {
        data.currentHealth -= amount;
        OnHealthChanged?.Invoke();
    }
    public void HandleLevelUp(int level)
    {
        data.currentHealth = GetMaxHealth();
        OnHealthChanged?.Invoke();
    }
    public float GetMaxHealth() => baseHealth * Mathf.Pow(hpLevelMultiplier, data.level - 1);
    public float GetCurrentHealth() => data.currentHealth;
    public bool IsDead() => data.currentHealth <= 0;
}
