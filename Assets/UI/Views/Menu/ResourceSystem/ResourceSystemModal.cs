using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSystemModal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button lockButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text costs;
    [SerializeField] private TMP_Text topCurrency;
    [SerializeField] private DrivesComponent drivesComponent;
    [SerializeField] private ResourceCostSection resourceCostSection;
    [SerializeField] private ResourceSection resourceSection;
    private PlayerService playerService;
    private void OnEnable()
    {
        playerService = GameManager.Instance.PlayerManager.playerService;

        UpdateText();
        UpdateCost();

        resourceSection.Setup();
        drivesComponent.Setup();
        resourceCostSection.Setup();

        lockButton.onClick.RemoveAllListeners();
        lockButton.onClick.AddListener(LockAction);
        playerService.OnCoinsChange += UpdateText;
        playerService.OnSpendDrives += UpdateCost;
    }

    private void OnDisable()
    {
        playerService.OnCoinsChange -= UpdateText;
        playerService.OnSpendDrives -= UpdateCost;
    }
    private void UpdateText()
    {
        topCurrency.text = playerService.GetCoins() + " Coins";
    }

    private void UpdateCost()
    {
        costs.text = playerService.GetResourceChargePerDrives().coins + " Coins";
        if (playerService.CanSpendDrives())
        {
            lockButton.interactable = true;
            buttonText.text = "Lock Resources";
        }
        else
        {
            lockButton.interactable = false;
            buttonText.text = "Not Enough Resources";
        }
    }

    private void LockAction()
    {
        playerService.SpendDrives(1);
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.Coins, SoundType.Coins_spend);
    }
}
