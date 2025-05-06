using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIController : MonoBehaviour
{
    public TMP_Text textLabel;
    public Slider progressSlider;
    public PlayerService instance;
    public void Setup(PlayerService player)
    {
        instance = player;
        textLabel.text = player.GetCurrentHealth().ToString();
        GameplayManager.Instance.spawner.ReportedKill += UpdateSlider;
    }
    public void UpdateSlider()
    {
        int currentKills = GameplayManager.Instance.spawner.playerKillCount;
        int requiredKills = GameplayManager.Instance.spawner.killsToNextWave;

        progressSlider.maxValue = requiredKills;
        progressSlider.value = currentKills;

        float percent = (float)currentKills / requiredKills * 100f;
        textLabel.text = Mathf.FloorToInt(percent) + "%";
    }
}
