using System;
using System.Collections.Generic;

public class ExpService
{
    private PlayerData data;
    public event Action OnLevelUp;
    public event Action OnExpGained;
    private LevelCurve levelCurve;
    public ExpService(PlayerData data)
    {
        this.data = data;
        levelCurve = LevelingSystem.OSCurve;
    }
    public void GainExp(int amount)
    {
        data.currentExp += amount;
        if (data.currentExp >= levelCurve.GetRequiredExp(data.level) && data.level < levelCurve.MaxLevel - 1)
        {
            data.level++;
            data.currentExp = 0;
            data.currentHealth = data.level * 5; // Multiply by hp multipler 
            OnLevelUp?.Invoke();
        }
        OnExpGained?.Invoke();
    }

    public int GetCurrentExp() => data.currentExp;
    public int GetRequiredExp() => levelCurve.GetRequiredExp(data.level);
    public int GetLevel() => data.level;
    public int GetMaxLevel() => levelCurve.MaxLevel;
}
