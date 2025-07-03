using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OSExpSlider : MonoBehaviour
{
    public TMP_Text textlabel;
    public Slider slider;
    public PlayerService instance;
    public void Setup(PlayerService instance)
    {
        this.instance = instance;
        UpdateSlider();
        instance.onGainExp += UpdateSlider;
    }
    public void UpdateSlider()
    {
        int maxValue = (int)instance.GetMaxExp();
        int value = (int)instance.GetCurrentExp();

        slider.maxValue = maxValue;
        slider.value = value;

        float percent = (float)value / maxValue * 100f;
        textlabel.text = Mathf.FloorToInt(percent) + "%";
    }
}
