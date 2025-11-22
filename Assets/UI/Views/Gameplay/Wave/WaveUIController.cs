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

    private void OnEnable()
    {
        SceneEventBus.Subscribe<WaveStartedEvent>(OnWaveStarted);
        SceneEventBus.Subscribe<WaveProgressUpdatedEvent>(OnWaveProgressUpdated);
    }

    private void OnDisable()
    {
        SceneEventBus.Unsubscribe<WaveStartedEvent>(OnWaveStarted);
        SceneEventBus.Unsubscribe<WaveProgressUpdatedEvent>(OnWaveProgressUpdated);
    }

    private void OnWaveStarted(WaveStartedEvent evt)
    {
        title.text = $"Wave {evt.WaveNumber}";
        progressSlider.maxValue = evt.EnemyCount;
        progressSlider.value = 0;
        textLabel.text = "0%";
    }

    private void OnWaveProgressUpdated(WaveProgressUpdatedEvent evt)
    {
        UpdateSlider(evt.CurrentKills, evt.RequiredKills, evt.WaveIndex);
    }

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
