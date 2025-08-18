using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOSHealthSlider : MonoBehaviour
{
    public TMP_Text textlabel;
    public Slider hpslider;
    public PlayerService instance;
    public void Setup(PlayerService instance)
    {
        this.instance = instance;
        textlabel.text = instance.GetCurrentHealth().ToString();
        UpdateSlider();
        instance.OnExpGained += UpdateSlider;
    }
    public void OnDisable()
    {
        instance.OnExpGained -= UpdateSlider;
    }
    public void UpdateSlider()
    {
        hpslider.maxValue = instance.GetMaxHealth();
        hpslider.value = instance.GetCurrentHealth();
        textlabel.text = instance.GetCurrentHealth().ToString();
    }
}
