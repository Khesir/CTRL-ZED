using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerService playerService { get; private set; }
    public async UniTask Initialize(PlayerData data)
    {
        // Todo Generate
        StartPlayerService(data);
        Debug.Log("[PlayerManager] Player Manager Initialized");
        await UniTask.CompletedTask;
    }
    public void StartPlayerService(PlayerData data)
    {
        // Currently static if possible to adjust level cure, do it later
        var expService = new ExpService(data);

        // Hp curve adjustment currently internal or other hp adjustments
        var healthService = new HealthService(data);

        // Adjust based on liking
        // Economy
        var baseCoins = 100;
        var costMultipler = 1.1f;
        var economyService = new EconomyService(data, baseCoins, costMultipler);

        var resourceService = new ResourceService(data);
        var bioChipService = new BioChipService(data);
        playerService = new PlayerService(
            data,
            expService,
            healthService,
            economyService,
            resourceService,
            bioChipService
        );
    }
}
