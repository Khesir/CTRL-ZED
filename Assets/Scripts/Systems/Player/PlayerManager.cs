using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] private PlayerService playerService;

    public event Action onCoinsChanged;
    public async UniTask Initialize(PlayerData data)
    {
        playerService = new PlayerService(data);

        await UniTask.CompletedTask;
    }
    public int GetPlayerCoins()
    {
        return playerService.GetPlayerCoins();
    }
    public bool SpendCoins(int amount)
    {
        var status = playerService.SpendCoins(amount);
        onCoinsChanged?.Invoke();
        return status;
    }
}
