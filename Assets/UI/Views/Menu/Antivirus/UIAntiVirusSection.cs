using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAntiVirusSection : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIAntivirusAbout uIAntivirusAbout;
    [SerializeField] private UIAntivirusSelector uIAntivirusSelector;
    [SerializeField] private TMP_Text currency;

    private IPlayerManager _playerManager;

    private void OnEnable()
    {
        _playerManager = ServiceLocator.Get<IPlayerManager>();

        List<StatusEffectData> antivirus = ServiceLocator.Get<IAntiVirusManager>().GetAllBuffs();
        uIAntivirusAbout.Setup(antivirus[0]);
        uIAntivirusSelector.Setup(uIAntivirusAbout, antivirus);
        UpdateCurrency();
        _playerManager.playerService.OnCoinsChange += UpdateCurrency;
    }

    private void OnDisable()
    {
        _playerManager.playerService.OnCoinsChange -= UpdateCurrency;
        uIAntivirusSelector.Clear();
    }

    private void UpdateCurrency()
    {
        currency.text = _playerManager.playerService.GetChargedDrives().ToString();
    }
}
