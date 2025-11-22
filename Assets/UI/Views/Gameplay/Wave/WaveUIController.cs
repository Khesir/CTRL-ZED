using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIController : MonoBehaviour
{
    public TMP_Text textLabel;
    public TMP_Text title;
    public Slider progressSlider;
    public void Setup()
    {
        title.text = $"Wave {GameplayManager.Instance.WaveManager.GetWaveIndex()}";
    }
    public void UpdateSlider(int currentKills, int requiredKills, int index)
    {
        progressSlider.maxValue = requiredKills;
        progressSlider.value = currentKills;

        float percent = (float)currentKills / requiredKills * 100f;
        textLabel.text = Mathf.FloorToInt(percent) + "%";
        title.text = $"Wave {index + 1}";
    }
}
