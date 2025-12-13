using System.Collections;
using System.Collections.Generic;
using Core.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIController : MonoBehaviour
{
    public TMP_Text textLabel;
    public TMP_Text title;
    public FillBarAnimator progressSlider;

    private void OnDisable()
    {
        SceneEventBus.Unsubscribe<WaveStartedEvent>(OnWaveStarted);
        SceneEventBus.Unsubscribe<WaveProgressUpdatedEvent>(OnWaveProgressUpdated);
    }

    private void OnWaveStarted(WaveStartedEvent evt)
    {
        title.text = $"Wave {evt.WaveNumber}";
        progressSlider.UpdateValues(0, evt.EnemyCount);
        textLabel.text = "0%";
    }

    private void OnWaveProgressUpdated(WaveProgressUpdatedEvent evt)
    {
        UpdateSlider(evt.CurrentKills, evt.RequiredKills, evt.WaveIndex);
    }

    public void Setup()
    {
        SceneEventBus.Subscribe<WaveStartedEvent>(OnWaveStarted);
        SceneEventBus.Subscribe<WaveProgressUpdatedEvent>(OnWaveProgressUpdated);

        title.text = $"Wave {ServiceLocator.Get<IWaveManager>().GetWaveIndex()}";
    }

    public void UpdateSlider(int currentKills, int requiredKills, int index)
    {
        progressSlider.UpdateValue(currentKills);

        float percent = (float)currentKills / requiredKills * 100f;
        textLabel.text = Mathf.FloorToInt(percent) + "%";
        title.text = $"Wave {index + 1}";
    }
}
