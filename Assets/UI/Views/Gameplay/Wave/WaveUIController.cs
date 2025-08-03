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
    public PlayerService instance;
    public void Setup(PlayerService player)
    {
        instance = player;
        textLabel.text = player.GetCurrentHealth().ToString();
        // GameplayManager.Instance.spawner.ReportedKill += UpdateSlider;
        // title.text = $"Wave {GameplayManager.Instance.spawner.waveLevel}";
    }
    public void UpdateSlider()
    {
        var spawner = GameplayManager.Instance.spawner;
        // int currentKills = spawner.playerKillCount;
        // int requiredKills = spawner.waves[spawner.waveNumber].requiredKills;

        // progressSlider.maxValue = requiredKills;
        // progressSlider.value = currentKills;

        // float percent = (float)currentKills / requiredKills * 100f;
        // textLabel.text = Mathf.FloorToInt(percent) + "%";
        // title.text = $"Wave {GameplayManager.Instance.spawner.waveLevel}";
    }
}
