using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core.Shared.Events;

public class UIRepairSection : MonoBehaviour
{
    public TMP_Text health;
    public TMP_Text cost;
    public TMP_Text level;
    public TMP_Text coins;
    public PlayerService instance;
    public Button button;
    public Slider exp;
    public int coinsNeeded;
    public void Setup(PlayerService instance)
    {
        this.instance = instance;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Repair);
        UpdateData();
        instance.OnHealthChanged += UpdateData;
    }
    public void OnDisable()
    {
        instance.OnHealthChanged -= UpdateData;
    }
    public void UpdateData()
    {
        float healthPerCoin = instance.GetHealthPerCoin();
        float maxHealth = instance.GetMaxHealth();
        float currentHealth = instance.GetCurrentHealth();
        float missingHealth = maxHealth - currentHealth;

        coinsNeeded = Mathf.CeilToInt(missingHealth / healthPerCoin);
        cost.text = coinsNeeded.ToString();

        coins.text = ServiceLocator.Get<IPlayerManager>().playerService.GetCoins() + " Coins";
        level.text = "Level " + instance.GetLevel();
        health.text = currentHealth.ToString();

        exp.maxValue = maxHealth;
        exp.value = currentHealth;
    }
    private void Repair()
    {
        float currentHealth = instance.GetCurrentHealth();
        float maxHealth = instance.GetMaxHealth();

        if (currentHealth >= maxHealth)
        {
            TooltipEventBus.PublishWarning("OS Health is already at maximum!");
            return;
        }

        int currentCoins = instance.GetCoins();

        if (currentCoins < coinsNeeded)
        {
            int shortage = coinsNeeded - currentCoins;
            TooltipEventBus.PublishError($"Not enough coins to repair! Need {shortage} more coins");
            return;
        }

        if (ServiceLocator.Get<IPlayerManager>().playerService.SpendCoins(coinsNeeded))
        {
            instance.Heal();
            TooltipEventBus.PublishSuccess($"OS repaired to full health! Spent {coinsNeeded} coins");
        }
        else
        {
            TooltipEventBus.PublishError("Failed to repair!");
        }
    }
}
