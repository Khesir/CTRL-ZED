using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateTeam : MonoBehaviour
{
    public TMP_Text priceText;
    private TeamManager manager;
    public void Initialize()
    {
        manager = GameManager.Instance.TeamManager;
        priceText.text = $"Buy for {manager.increaseSizePrice} coins";
    }
    public void CreateTeamGroup()
    {
        var res = GameManager.Instance.ResourceManager.SpendCoins(manager.increaseSizePrice);
        if (res)
        {
            manager.IncreaseMaxTeam();
            manager.CreateTeam();
        }
    }
}
