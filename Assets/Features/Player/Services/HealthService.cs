using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthService : IHealthService
{
    private PlayerData data;
    private const float LevelScalingFactor = 0.1f; // 10% stronger per level (matches enemy/character scaling)
    private const float baseHealth = 100f;
    public event Action OnHealthChanged;

    public HealthService(PlayerData data)
    {
        this.data = data;
    }

    // Linear scaling that matches enemy and character progression
    private float GetLevelMultiplier() => 1 + (data.level * LevelScalingFactor);

    public void Heal()
    {
        data.currentHealth = GetMaxHealth();
        OnHealthChanged?.Invoke();
    }

    public void HealAmount(float amount)
    {
        data.currentHealth = Mathf.Min(data.currentHealth + amount, GetMaxHealth());
        OnHealthChanged?.Invoke();
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
    public float GetMaxHealth() => baseHealth * GetLevelMultiplier();
    public float GetCurrentHealth()
    {
        if (data.currentHealth == -1)
        {
            data.currentHealth = GetMaxHealth();
        }
        return data.currentHealth;
    }
    public bool IsDead() => data.currentHealth <= 0;
}
