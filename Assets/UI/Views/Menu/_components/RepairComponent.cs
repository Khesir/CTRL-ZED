using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairComponent : MonoBehaviour
{
    public TMP_Text value;
    public Slider slider;
    public PlayerService service;
    public void Setup()
    {
        service = GameManager.Instance.PlayerManager.playerService;
        UpdateSlider();
        service.OnHealthChanged += UpdateSlider;
    }
    public void UpdateSlider()
    {
        int maxValue = (int)service.GetMaxHealth();
        int value = (int)service.GetCurrentHealth();

        slider.maxValue = maxValue;
        slider.value = value;

        float percent = (float)value / maxValue * 100f;
        this.value.text = Mathf.FloorToInt(percent) + "%";
    }
}
