using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Core.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOSHealthBar : MonoBehaviour
{
    [SerializeField] private TMP_Text textlabel;
    [SerializeField] private FillBarAnimator healthbar;

    private PlayerService playerService;

    public void Setup(PlayerService service)
    {
        playerService = service;

        // Initialize with max health and current health
        float currentHealth = playerService.GetCurrentHealth();
        float maxHealth = playerService.GetMaxHealth();

        healthbar.Initialize(maxHealth, currentHealth);
        UpdateLabel(currentHealth, maxHealth);

        // Subscribe to events
        CoreEventBus.Subscribe<PlayerHealthChangedEvent>(OnHealthChanged);
        CoreEventBus.Subscribe<PlayerLevelUpEvent>(OnLevelUp);
    }

    public void OnDisable()
    {
        CoreEventBus.Unsubscribe<PlayerHealthChangedEvent>(OnHealthChanged);
        CoreEventBus.Unsubscribe<PlayerLevelUpEvent>(OnLevelUp);
    }

    private void OnHealthChanged(PlayerHealthChangedEvent evt)
    {
        float currentHealth = evt.CurrentHealth;
        float maxHealth = evt.MaxHealth;

        healthbar.UpdateValue(currentHealth);
        UpdateLabel(currentHealth, maxHealth);
    }

    private void OnLevelUp(PlayerLevelUpEvent evt)
    {
        // When player levels up, max health changes
        float currentHealth = playerService.GetCurrentHealth();
        float maxHealth = playerService.GetMaxHealth();

        healthbar.UpdateValues(currentHealth, maxHealth);
        UpdateLabel(currentHealth, maxHealth);
    }

    private void UpdateLabel(float current, float max)
    {
        if (textlabel != null)
        {
            textlabel.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)} HP";
        }
    }
}
