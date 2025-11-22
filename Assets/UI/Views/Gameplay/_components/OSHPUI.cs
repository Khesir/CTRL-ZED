using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OSHPUI : MonoBehaviour
{
    public TMP_Text textLabel;
    public Slider hpSlider;
    public PlayerService instance;

    private void OnEnable()
    {
        // Subscribe to CoreEventBus for cross-scene health updates
        CoreEventBus.Subscribe<PlayerHealthChangedEvent>(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        CoreEventBus.Unsubscribe<PlayerHealthChangedEvent>(OnPlayerHealthChanged);

        // Keep backward compatibility with direct subscription
        if (instance != null)
            instance.OnHealthChanged -= UpdateSlider;
    }

    private void OnPlayerHealthChanged(PlayerHealthChangedEvent evt)
    {
        hpSlider.maxValue = evt.MaxHealth;
        hpSlider.value = evt.CurrentHealth;
        textLabel.text = evt.CurrentHealth.ToString();
    }

    public void Setup(PlayerService player)
    {
        instance = player;
        textLabel.text = player.GetCurrentHealth().ToString();
        UpdateSlider();
        // Keep direct subscription for initial setup compatibility
        instance.OnHealthChanged += UpdateSlider;
    }

    public void UpdateSlider()
    {
        if (instance == null) return;
        hpSlider.maxValue = instance.GetMaxHealth();
        hpSlider.value = instance.GetCurrentHealth();
        textLabel.text = instance.GetCurrentHealth().ToString();
    }
}
