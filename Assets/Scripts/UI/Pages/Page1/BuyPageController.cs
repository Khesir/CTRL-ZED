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
        var playerManager = GameManager.Instance.PlayerManager;
        var characterManager = GameManager.Instance.CharacterManager;

        buyInventoryUI.Populate(characterManager.characterTemplates);
        coinCounter.Setup(playerManager.GetPlayerCoins());

        GameManager.Instance.PlayerManager.onCoinsChanged += coinCounter.UpdateCoins;
    }
    private void OnDisable()
    {
        GameManager.Instance.PlayerManager.onCoinsChanged -= coinCounter.UpdateCoins;
    }
}
