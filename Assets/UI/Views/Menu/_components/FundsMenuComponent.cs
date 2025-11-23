using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FundsMenuComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text value;
    public PlayerService service;
    public void Setup()
    {
        service = ServiceLocator.Get<IPlayerManager>().playerService;
        UpdateValue();
        service.OnCoinsChange += UpdateValue;
    }
    public void UpdateValue()
    {
        value.text = service.GetCoins().ToString();
    }
}
