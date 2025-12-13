using System.Collections;
using System.Collections.Generic;
using Core.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OSHPUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private FillBarAnimator fillBar;
    [SerializeField] private PlayerService instance;
    private void OnDisable()
    {
        CoreEventBus.Unsubscribe<PlayerHealthChangedEvent>(OnHealthChanged);
    }

    public void Setup(PlayerService player)
    {
        instance = player;

        float currentHealth = player.GetCurrentHealth();
        float maxHealth = player.GetMaxHealth();

        fillBar.Initialize(maxHealth, currentHealth);
        UpdateLabel(currentHealth, maxHealth);
        CoreEventBus.Subscribe<PlayerHealthChangedEvent>(OnHealthChanged);
    }
    private void UpdateLabel(float current, float max)
    {
        if (textLabel != null)
        {
            textLabel.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)} HP";
        }
    }
    private void OnHealthChanged(PlayerHealthChangedEvent evt)
    {
        float currentHealth = evt.CurrentHealth;
        float maxHealth = evt.MaxHealth;

        fillBar.UpdateValue(currentHealth);
        UpdateLabel(currentHealth, maxHealth);
    }
}
