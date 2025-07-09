using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyService : IEconomyService
{
    private PlayerData data;

    // Internal tuning parameters
    private readonly int baseRequiredCoins;
    private readonly float coinRequirementMultiplier;

    public EconomyService(PlayerData data, int baseCoins = 100, float multiplier = 1.25f)
    {
        this.data = data;
        baseRequiredCoins = baseCoins;
        coinRequirementMultiplier = multiplier;
    }
    public float GetCoinsPerExp() => data.coinsPerExp;
    public float GetHealthPerCoin() => data.healthPerCoin;
    public int GetRequiredCoinsToLevelup()
    {
        int level = data.level;
        // Exponential curve based on level
        float requiredCoins = baseRequiredCoins * Mathf.Pow(coinRequirementMultiplier, level - 1);
        return Mathf.CeilToInt(requiredCoins);
    }
}
