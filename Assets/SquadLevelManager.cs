using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLevelManager : MonoBehaviour
{
    public int maxLevel;
    public int currentLevel;
    public int experience;
    public List<int> playerLevels = new();
    public event Action onGainExperience;
    public void Setup(int maxLevel = 100)
    {
        this.maxLevel = maxLevel;
        playerLevels.Clear();

        int baseXP = 100;
        playerLevels.Add(baseXP);

        for (int i = 1; i < maxLevel; i++)
        {
            int nextXP = Mathf.CeilToInt(playerLevels[i - 1] * 1.1f);
            playerLevels.Add(nextXP);
        }
        Debug.Log("SquaLevel Ready");
    }

    public void GetExperience(int experienceToGet)
    {
        experience += experienceToGet;
        if (experience >= playerLevels[currentLevel])
        {
            currentLevel++;
            experience = 0;
        }
        onGainExperience?.Invoke();
    }
}
