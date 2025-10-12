using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAntiVirusSection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIAntivirusAbout uIAntivirusAbout;

    [SerializeField] private UIAntivirusSelector uIAntivirusSelector;
    [SerializeField] private TMP_Text currency;
    private void OnEnable()
    {
        List<StatusEffectData> antivirus = GameManager.Instance.AntiVirusManager.GetAllBuffs();
        uIAntivirusAbout.Setup(antivirus[0]);
        uIAntivirusSelector.Setup(uIAntivirusAbout, antivirus);
        UpdateCurrency();
        GameManager.Instance.PlayerManager.playerService.OnCoinsChange += UpdateCurrency;
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerManager.playerService.OnCoinsChange -= UpdateCurrency;
        uIAntivirusSelector.Clear();
    }
    private void UpdateCurrency()
    {
        currency.text = GameManager.Instance.PlayerManager.playerService.GetChargedDrives().ToString();
    }
}
