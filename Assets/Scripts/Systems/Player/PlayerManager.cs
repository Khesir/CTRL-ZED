using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] private PlayerData playerData;
    [HideInInspector] private PlayerService playerService;

    public event Action onCoinsChanged;
    public event Action onInventoryChange;
    public async UniTask Initialize()
    {
        // Try to load from file
        // playerData = ServiceContainer.LoadPlayerData();
        if (playerData != null)
        {
            playerService = new PlayerService(playerData);
        }
        else
        {
            playerData = new PlayerData();
            playerService = new PlayerService(playerData);
        }
        await UniTask.CompletedTask;
    }
    public List<CharacterData> GetOwnedCharacters()
    {
        return playerService.GetOwnedCharacters();
    }
    public int GetPlayerCoins()
    {
        return playerService.GetPlayerCoins();
    }
    public void AddCharacter(CharacterData characterData)
    {
        playerService.AddCharacter(characterData);
    }
    public PurchaseResult PurchaseCharacter(CharacterConfig character)
    {
        PurchaseResult res = playerService.PurchaseCharacter(character);

        if (res.Success)
        {
            onCoinsChanged?.Invoke();
            onInventoryChange?.Invoke();
        }

        return res;
    }
}

