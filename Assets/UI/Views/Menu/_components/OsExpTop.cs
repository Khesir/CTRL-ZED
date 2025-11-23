using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OsExpTop : MonoBehaviour
{
    public TMP_Text level;
    public Slider expSlider;
    public TMP_Text expText;
    public PlayerService service;
    public void Setup()
    {
        service = ServiceLocator.Get<IPlayerManager>().playerService;
        UpdateText();

        UpdateSlider();
        service.OnExpGained += UpdateSlider;
        service.OnLevelUp += UpdateText;
    }
    public void OnDestroy()
    {
        service.OnExpGained -= UpdateSlider;
        service.OnLevelUp -= UpdateText;
    }
    public void UpdateSlider()
    {
        int maxValue = (int)service.GetRequiredExp();
        int value = (int)service.GetCurrentExp();
        expSlider.maxValue = maxValue;
        expSlider.value = value;
        float percent = (float)value / maxValue * 100f;
        expText.text = Mathf.FloorToInt(percent) + "%";
    }

    private void UpdateText()
    {
        level.text = service.GetLevel().ToString();
    }
}
