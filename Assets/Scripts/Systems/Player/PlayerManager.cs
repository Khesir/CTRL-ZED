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
    public PlayerService GetPlayerService()
    {
        return playerService;
    }
}
