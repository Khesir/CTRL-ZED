using System.Collections;
using System.Collections.Generic;
using Core.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class UIOSExpBar : MonoBehaviour
{
    [SerializeField] private TMP_Text textlabel;
    [SerializeField] private FillBarAnimator expbar;

    private PlayerService playerService;

    public void Setup(PlayerService service)
    {
        playerService = service;

        // Initialize with max exp (to next level) and current exp
        int currentExp = playerService.GetCurrentExp();
        int requiredExp = playerService.GetRequiredExp();

        expbar.Initialize(requiredExp, currentExp);
        UpdateLabel(currentExp, requiredExp);

        // Subscribe to events
        CoreEventBus.Subscribe<PlayerExpGainEvent>(OnExpGained);
        CoreEventBus.Subscribe<PlayerLevelUpEvent>(OnLevelUp);
    }

    public void OnDisable()
    {
        CoreEventBus.Unsubscribe<PlayerExpGainEvent>(OnExpGained);
        CoreEventBus.Unsubscribe<PlayerLevelUpEvent>(OnLevelUp);
    }

    private void OnExpGained(PlayerExpGainEvent evt)
    {
        int currentExp = playerService.GetCurrentExp();
        int requiredExp = playerService.GetRequiredExp();

        expbar.UpdateValue(currentExp);
        UpdateLabel(currentExp, requiredExp);
    }

    private async void OnLevelUp(PlayerLevelUpEvent evt)
    {
        int oldRequiredExp = playerService.GetRequiredExp();
        int currentExp = playerService.GetCurrentExp();

        // Animate overflow (fill to max, then reset)
        await expbar.AnimateOverflow(currentExp);

        // Update max for new level
        int newRequiredExp = playerService.GetRequiredExp();
        expbar.UpdateMaxValue(newRequiredExp, instant: true);
        UpdateLabel(currentExp, newRequiredExp);
    }

    private void UpdateLabel(int current, int required)
    {
        if (textlabel != null)
        {
            textlabel.text = $"{current} / {required} EXP";
        }
    }
}
