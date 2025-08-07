using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadLevelUI : MonoBehaviour
{
    public TMP_Text textLabel;
    public Slider hpSlider;
    public void Setup()
    {
        textLabel.text = GameplayManager.Instance.squadLevelManager.experience.ToString();
        // GameplayManager.Instance.squadLevelManager.onGainExperience += UpdateSlider;
    }
    public void UpdateSlider()
    {
        var manager = GameplayManager.Instance.squadLevelManager;
        hpSlider.maxValue = manager.playerLevels[manager.currentLevel];
        hpSlider.value = manager.experience;
        textLabel.text = GameplayManager.Instance.squadLevelManager.currentLevel.ToString();
    }
}
