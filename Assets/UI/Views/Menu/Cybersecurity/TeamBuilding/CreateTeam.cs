using TMPro;
using UnityEngine;

public class CreateTeam : MonoBehaviour
{
    public TMP_Text priceText;
    private ITeamManager _teamManager;

    public void Initialize()
    {
        _teamManager = ServiceLocator.Get<ITeamManager>();
        priceText.text = $"Buy for {_teamManager.increaseSizePrice} coins";
    }

    public void CreateTeamGroup()
    {
        var res = ServiceLocator.Get<IPlayerManager>().playerService.SpendCoins(_teamManager.increaseSizePrice);
        if (res)
        {
            _teamManager.IncreaseMaxTeam();
            _teamManager.CreateTeam();
            ServiceLocator.Get<ISoundService>().Play(SoundCategory.Coins, SoundType.Coins_massive);
            return;
        }
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_Error);
    }
}
