using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OsExpTop : MonoBehaviour
{
    [SerializeField] private TMP_Text level;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text expText;

    private PlayerService service;

    public void Setup()
    {
        service = ServiceLocator.Get<IPlayerManager>()?.playerService;
        if (service == null)
        {
            Debug.LogError("[OsExpTop] PlayerService is null!");
            return;
        }

        UpdateText();
        UpdateSlider();

        service.OnExpGained += UpdateSlider;
        service.OnLevelUp += UpdateText;
    }

    private void OnDestroy()
    {
        if (service != null)
        {
            service.OnExpGained -= UpdateSlider;
            service.OnLevelUp -= UpdateText;
        }
    }

    public void UpdateSlider()
    {
        if (service == null || expSlider == null || expText == null) return;

        int maxValue = (int)service.GetRequiredExp();
        int value = (int)service.GetCurrentExp();
        expSlider.maxValue = maxValue;
        expSlider.value = value;
        float percent = (float)value / maxValue * 100f;
        expText.text = Mathf.FloorToInt(percent) + "%";
    }

    private void UpdateText()
    {
        if (service == null || level == null) return;

        level.text = service.GetLevel().ToString();
    }
}
