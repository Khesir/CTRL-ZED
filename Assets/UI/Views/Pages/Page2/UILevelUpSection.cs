using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUpSection : MonoBehaviour
{
    public TMP_Text requirements;
    public TMP_Text cost;
    public TMP_Text level;
    public TMP_Text coins;
    public PlayerService instance;
    public Button button;
    public Slider exp;
    public int costCoins;
    public void Setup(PlayerService instance)
    {
        this.instance = instance;
        UpdateData();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Levelup);
        instance.OnExpGained += UpdateData;
    }
    private void UpdateData()
    {
        int currentExp = instance.GetCurrentExp();
        int requiredExp = instance.GetRequiredExp();
        int remainingExp = Mathf.Max(requiredExp - currentExp, 0);
        exp.maxValue = remainingExp;
        exp.value = currentExp;
        float coinsPerExp = instance.GetCoinsPerExpRate();
        costCoins = Mathf.CeilToInt(remainingExp * coinsPerExp);

        int currentCoins = instance.GetCoins();

        // Update UI
        requirements.text = $"{currentExp}/{requiredExp}";
        level.text = "Level " + instance.GetLevel();
        coins.text = currentCoins + " Coins";
        cost.text = costCoins + " Coins";
    }
    public void Levelup()
    {
        float coinsPerExp = instance.GetCoinsPerExpRate();
        int remainingExp = (int)(costCoins / coinsPerExp);
        if (instance.SpendCoins(costCoins))
        {
            instance.GainExp(remainingExp);
        }
    }
}
