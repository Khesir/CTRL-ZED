using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelingSystem
{
    private static readonly List<int> XPTable = new();
    private static int maxLevel = 100;
    static LevelingSystem()
    {
        Debug.Log("Static constructor called!");
        int baseXP = 50;
        XPTable.Add(baseXP);
        for (int i = 1; i < maxLevel; i++)
        {
            XPTable.Add(Mathf.CeilToInt(XPTable[i - 1] * 1.1f));
        }
    }
    public static int GetMaxLevel() => maxLevel;
    public static int GetRequiredExp(int level) => XPTable[level];

    public static bool CanLevelUp(int currentExp, int currentLevel) => currentExp >= GetRequiredExp(currentLevel);
}
