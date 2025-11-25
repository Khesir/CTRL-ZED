using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core.Shared.Events;

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
    public void OnDisable()
    {
        instance.OnExpGained -= UpdateData;
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
        level.text = instance.GetLevel().ToString();
        coins.text = currentCoins.ToString();
        cost.text = costCoins.ToString();
    }
    public void Levelup()
    {
        float coinsPerExp = instance.GetCoinsPerExpRate();
        int remainingExp = (int)(costCoins / coinsPerExp);
        int currentCoins = instance.GetCoins();

        if (currentCoins < costCoins)
        {
            int shortage = costCoins - currentCoins;
            TooltipEventBus.PublishError($"Not enough coins to level up! Need {shortage} more coins");
            return;
        }

        if (instance.SpendCoins(costCoins))
        {
            instance.GainExp(remainingExp);
            TooltipEventBus.PublishSuccess($"Level up successful! Spent {costCoins} coins");
        }
        else
        {
            TooltipEventBus.PublishError("Failed to level up!");
        }
    }
}
