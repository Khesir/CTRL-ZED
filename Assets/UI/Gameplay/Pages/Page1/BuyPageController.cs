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
        buyInventoryUI.Populate(GameManager.Instance.characterTemplates);
        coinCounter.Setup(GameManager.Instance.ResourceManager.GetCoins());

        GameManager.Instance.ResourceManager.onCoinsChange += coinCounter.UpdateCoins;
    }
    private void OnDisable()
    {
        GameManager.Instance.PlayerManager.onCoinsChanged -= coinCounter.UpdateCoins;
    }
}
