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
    public event Action onInventoryChange;
    public async UniTask Initialize(PlayerData playerData)
    {
        playerService = new PlayerService(playerData);

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

