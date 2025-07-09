using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : IResourceService, IEconomyService, IExpService, IHealthService
{
    private PlayerData data;

    public ExpService expService { get; private set; }
    public HealthService healthService { get; private set; }
    public EconomyService economyService { get; private set; }
    public ResourceService resourceService { get; private set; }
    public event Action OnHealthChanged;
    public event Action OnLevelUp;
    public event Action OnExpGained;

    public PlayerService(PlayerData data, ExpService expService, HealthService healthService, EconomyService economyService, ResourceService resourceService)
    {
        this.data = data;
        this.expService = expService;
        this.healthService = healthService;
        this.economyService = economyService;
        this.resourceService = resourceService;

        WiredEvents();
        PlayerHandleEvents();
    }

    public void WiredEvents()
    {
        healthService.OnHealthChanged += () => OnHealthChanged?.Invoke();
        // Exp Events
        expService.OnLevelUp += () => OnLevelUp?.Invoke();
        expService.OnExpGained += () => OnExpGained?.Invoke();
    }
    public void PlayerHandleEvents()
    {
        OnLevelUp += () => healthService.HandleLevelUp(data.level);
    }
    #region ExperienceService
    public void GainExp(int amount) => expService.GainExp(amount);
    public int GetCurrentExp() => expService.GetCurrentExp();
    public int GetRequiredExp() => expService.GetRequiredExp();
    public int GetLevel() => expService.GetLevel();
    public int GetMaxLevel() => expService.GetLevel();
    #endregion

    #region HealthService
    public void Heal() => healthService.Heal();
    public void TakeDamage(float damage) => healthService.TakeDamage(damage);
    public void HandleLevelUp(int level) => healthService.HandleLevelUp(data.level);
    public float GetMaxHealth() => healthService.GetMaxHealth();
    public float GetCurrentHealth() => healthService.GetCurrentHealth();
    public bool IsDead() => healthService.IsDead();
    #endregion

    #region EconomyService
    public float GetCoinsPerExp() => economyService.GetCoinsPerExp();
    public float GetHealthPerCoin() => economyService.GetHealthPerCoin();
    public int GetRequiredCoinsToLevelup() => economyService.GetRequiredCoinsToLevelup();
    public int GetCoins() => economyService.GetCoins();
    public void AddCoins(int val) => economyService.AddCoins(val);
    public bool SpendCoins(int val) => economyService.SpendCoins(val);
    #endregion

    #region ResourceService
    // Food
    public int GetFood() => resourceService.GetFood();
    public void SpendFood(int val) => resourceService.SpendFood(val);
    public void AddFood(int val) => resourceService.AddFood(val);

    // Technology
    public int GetTechnology() => resourceService.GetTechnology();
    public void SpendTechnology(int val) => resourceService.SpendTechnology(val);
    public void AddTechnology(int val) => resourceService.AddTechnology(val);

    // Energy
    public int GetEnergy() => resourceService.GetEnergy();
    public void SpendEnergy(int val) => resourceService.SpendEnergy(val);
    public void AddEnergy(int val) => resourceService.AddEnergy(val);

    // Intelligence
    public int GetIntelligence() => resourceService.GetIntelligence();
    public void SpendIntelligence(int val) => resourceService.SpendIntelligence(val);
    public void AddIntelligence(int val) => resourceService.AddIntelligence(val);

    #endregion
}
