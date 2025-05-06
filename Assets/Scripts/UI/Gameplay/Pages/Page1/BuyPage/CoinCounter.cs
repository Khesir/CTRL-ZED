using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void Setup(int counter)
    {
        text.text = counter.ToString();
    }
    public void UpdateCoins() => Setup(GameManager.Instance.PlayerManager.GetPlayerCoins());

}
