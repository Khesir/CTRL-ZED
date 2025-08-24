using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelCurve
{
    int GetRequiredExp(int level);
    int MaxLevel { get; }
}

public class LevelCurve : ILevelCurve
{
    private readonly List<int> xpTable = new();
    private readonly List<int> costCurve = new();
    public int MaxLevel { get; }

    public LevelCurve(int maxLevel, int baseXP, float growthRate)
    {
        MaxLevel = maxLevel;
        xpTable.Add(baseXP);
        for (int i = 1; i < maxLevel; i++)
        {
            xpTable.Add(Mathf.CeilToInt(xpTable[i - 1] * growthRate));
        }
        // Cost Curve -- temporary solution for drives
        int baseCost = xpTable[1];

        for (int i = 1; i < xpTable.Count; i++)
        {
            int cost = xpTable[i] / baseCost;
            costCurve.Add(cost == 0 ? 1 : cost);
        }
    }
    public int GetRequiredExp(int level)
    {
        return xpTable[Mathf.Clamp(level, 0, MaxLevel - 1)];
    }
    // Optional
    public int GetCostCurve(int level)
    {

        return costCurve[Mathf.Clamp(level, 1, MaxLevel - 1)];
    }
}