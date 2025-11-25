using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core.Shared.Events;
using Core.Shared.UI;

public class OSPageController : MonoBehaviour
{
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button repairButton;
    [SerializeField] private UIOSExpBar oSExpBar;
    [SerializeField] private UIOSHealthBar oSHealthBar;
    [SerializeField] private TMP_Text level;

    [Header("Money Display")]
    [SerializeField] private TMP_Text money;
    [SerializeField] private AnimatedMoneyDisplay animatedMoney; // Optional: Use animated version

    private PlayerService service;

    private void OnEnable()
    {
        service = ServiceLocator.Get<IPlayerManager>().playerService;
        InternalUpdateText(service.GetLevel());
        UpdateMoneyDisplay();

        // Setup bars with PlayerService (they handle initialization and events)
        oSExpBar.Setup(service);
        oSHealthBar.Setup(service);

        // Level up Service
        levelUpButton.onClick.RemoveAllListeners();
        levelUpButton.onClick.AddListener(LevelUpAction);

        repairButton.onClick.RemoveAllListeners();
        repairButton.onClick.AddListener(RepairAction);

        CoreEventBus.Subscribe<PlayerLevelUpEvent>(UpdateText);
        CoreEventBus.Subscribe<PlayerDataChangedEvent>(OnCoinsChanged);
    }

    private void OnDisable()
    {
        CoreEventBus.Unsubscribe<PlayerLevelUpEvent>(UpdateText);
        CoreEventBus.Unsubscribe<PlayerDataChangedEvent>(OnCoinsChanged);
    }

    private void LevelUpAction()
    {
        // LevelUp logic
        // - Converts money to level up os
        // - should be validated if there is enough amount else send a tooltip message event

        int currentExp = service.GetCurrentExp();
        int requiredExp = service.GetRequiredExp();
        int remainingExp = Mathf.Max(requiredExp - currentExp, 0);

        if (remainingExp <= 0)
        {
            TooltipEventBus.PublishWarning("Already at maximum experience for current level!");
            return;
        }

        float coinsPerExp = service.GetCoinsPerExpRate();
        int costCoins = Mathf.CeilToInt(remainingExp * coinsPerExp);
        int currentCoins = service.GetCoins();

        if (currentCoins < costCoins)
        {
            int shortage = costCoins - currentCoins;
            TooltipEventBus.PublishError($"Not enough coins to level up! Need {shortage} more coins");
            return;
        }

        // Spend coins and gain experience
        if (service.SpendCoins(costCoins))
        {
            service.GainExp(remainingExp);
            TooltipEventBus.PublishSuccess($"Level up successful! Spent {costCoins} coins");
        }
        else
        {
            TooltipEventBus.PublishError("Failed to spend coins!");
        }
    }

    private void RepairAction()
    {
        // Repair logic
        // - heals player oshp based on the amount paid,
        // - should validated if there is enough amount else send a tooltip message event.

        float currentHealth = service.GetCurrentHealth();
        float maxHealth = service.GetMaxHealth();
        float missingHealth = maxHealth - currentHealth;

        if (missingHealth <= 0)
        {
            TooltipEventBus.PublishWarning("OS Health is already at maximum!");
            return;
        }

        float healthPerCoin = service.GetHealthPerCoin();
        int coinsNeeded = Mathf.CeilToInt(missingHealth / healthPerCoin);
        int currentCoins = service.GetCoins();

        if (currentCoins < coinsNeeded)
        {
            int shortage = coinsNeeded - currentCoins;
            TooltipEventBus.PublishError($"Not enough coins to repair! Need {shortage} more coins");
            return;
        }

        // Spend coins and heal
        if (service.SpendCoins(coinsNeeded))
        {
            service.Heal();
            TooltipEventBus.PublishSuccess($"OS repaired to full health! Spent {coinsNeeded} coins");
        }
        else
        {
            TooltipEventBus.PublishError("Failed to spend coins!");
        }
    }
    private void UpdateText(PlayerLevelUpEvent evt) => InternalUpdateText(evt.NewLevel);

    private void InternalUpdateText(float val)
    {
        level.text = val.ToString();
    }

    private void OnCoinsChanged(PlayerDataChangedEvent evt)
    {
        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        int currentCoins = service.GetCoins();

        // Use animated display if available, otherwise use simple text
        if (animatedMoney != null)
        {
            animatedMoney.UpdateAmount(currentCoins);
        }
        else if (money != null)
        {
            money.text = $"{currentCoins} Coins";
        }
    }

    /// <summary>
    /// Call this when hovering over level up button to preview cost
    /// </summary>
    public void PreviewLevelUpCost()
    {
        int currentExp = service.GetCurrentExp();
        int requiredExp = service.GetRequiredExp();
        int remainingExp = Mathf.Max(requiredExp - currentExp, 0);

        if (remainingExp <= 0) return;

        float coinsPerExp = service.GetCoinsPerExpRate();
        int costCoins = Mathf.CeilToInt(remainingExp * coinsPerExp);

        ShowCostPreview(costCoins);
    }

    /// <summary>
    /// Call this when hovering over repair button to preview cost
    /// </summary>
    public void PreviewRepairCost()
    {
        float currentHealth = service.GetCurrentHealth();
        float maxHealth = service.GetMaxHealth();
        float missingHealth = maxHealth - currentHealth;

        if (missingHealth <= 0) return;

        float healthPerCoin = service.GetHealthPerCoin();
        int coinsNeeded = Mathf.CeilToInt(missingHealth / healthPerCoin);

        ShowCostPreview(coinsNeeded);
    }

    /// <summary>
    /// Stop showing cost preview and return to normal display
    /// </summary>
    public void ClearCostPreview()
    {
        if (animatedMoney != null)
        {
            animatedMoney.DisplayNormal();
        }
        else
        {
            UpdateMoneyDisplay();
        }
    }

    private void ShowCostPreview(int cost)
    {
        int currentCoins = service.GetCoins();

        if (animatedMoney != null)
        {
            animatedMoney.PreviewCost(cost);
        }
        else if (money != null)
        {
            int resultAmount = currentCoins - cost;
            money.text = $"{currentCoins} - {cost} = {resultAmount} Coins";
        }
    }
}
