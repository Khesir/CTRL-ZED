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
    public int MaxLevel { get; }

    public LevelCurve(int maxLevel, int baseXP, float growthRate)
    {
        MaxLevel = maxLevel;
        xpTable.Add(baseXP);
        for (int i = 1; i < maxLevel; i++)
        {
            xpTable.Add(Mathf.CeilToInt(xpTable[i - 1] * growthRate));
        }
    }
    public int GetRequiredExp(int level)
    {
        return xpTable[Mathf.Clamp(level, 0, MaxLevel - 1)];
    }
}