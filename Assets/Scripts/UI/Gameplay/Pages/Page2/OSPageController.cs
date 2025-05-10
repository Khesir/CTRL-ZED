using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OSPageController : MonoBehaviour
{
    public Button levelUpButton;
    public Button repairButton;
    public OSExpSlider oSExpSlider;
    public OSHealthSlider oSHealthSlider;
    public TMP_Text level;
    public PlayerService service;

    [Header("SubPages")]
    public UILevelUpSection UILevelUpSection;
    public UIRepairSection UIRepairSection;
    private void OnEnable()
    {
        service = GameManager.Instance.PlayerManager.GetPlayerService();

        oSExpSlider.Setup(service);
        oSHealthSlider.Setup(service);

        // Level up Service
        levelUpButton.onClick.RemoveAllListeners();
        levelUpButton.onClick.AddListener(LevelUpAction);

        repairButton.onClick.RemoveAllListeners();
        repairButton.onClick.AddListener(RepairAction);

        service.onGainExp += UpdateText;
    }
    private void OnDisable()
    {
        service.onGainExp -= UpdateText;
    }
    private void LevelUpAction()
    {
        UILevelUpSection.gameObject.SetActive(true);
        UILevelUpSection.Setup(service);
    }
    private void RepairAction()
    {
        UIRepairSection.gameObject.SetActive(true);
        UIRepairSection.Setup(service);
    }
    private void UpdateText()
    {
        level.text = service.GetLevel() + "/" + service.GetMaxLevel();
    }
}
