using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPageController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BuyInventoryUI buyInventoryUI;
    [SerializeField] private CoinCounter coinCounter;
    private void OnEnable()
    {
        buyInventoryUI.Populate(GameManager.Instance.CharacterManager.characterTemplates);
        coinCounter.Setup(GameManager.Instance.PlayerManager.playerService.GetCoins());

        GameManager.Instance.PlayerManager.playerService.OnCoinsChange += coinCounter.UpdateCoins;
    }
    private void OnDisable()
    {
        GameManager.Instance.PlayerManager.playerService.OnCoinsChange -= coinCounter.UpdateCoins;
    }
}
