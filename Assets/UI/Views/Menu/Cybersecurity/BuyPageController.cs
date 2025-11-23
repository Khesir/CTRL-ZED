using UnityEngine;

public class BuyPageController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BuyInventoryUI buyInventoryUI;
    [SerializeField] private CoinCounter coinCounter;

    private ICharacterManager _characterManager;
    private IPlayerManager _playerManager;

    private void OnEnable()
    {
        _characterManager = ServiceLocator.Get<ICharacterManager>();
        _playerManager = ServiceLocator.Get<IPlayerManager>();

        buyInventoryUI.Populate(_characterManager.characterTemplates);
        coinCounter.Setup(_playerManager.playerService.GetCoins());

        _playerManager.playerService.OnCoinsChange += coinCounter.UpdateCoins;
    }

    private void OnDisable()
    {
        _playerManager.playerService.OnCoinsChange -= coinCounter.UpdateCoins;
    }
}
