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
    public void Setup(PlayerService player)
    {
        instance = player;
        textLabel.text = player.GetCurrentHealth().ToString();
        UpdateSlider();
        player.OnHealthChanged += UpdateSlider;
    }
    public void UpdateSlider()
    {
        hpSlider.maxValue = instance.GetMaxHealth();
        hpSlider.value = instance.GetCurrentHealth();
        textLabel.text = instance.GetCurrentHealth().ToString();
    }
}
