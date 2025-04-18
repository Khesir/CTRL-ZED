using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PurchaseResult
{
    public bool Success { get; }
    public string Message { get; }

    public PurchaseResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData;
    public int coins => playerData.coins;
    public List<CharacterInstance> OwnedCharacters => playerData.ownedCharacters;

    public event Action onCoinsChanged;
    public event Action onInventoryChange;
    public async UniTask Initialize()
    {
        if (playerData == null)
        {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
        }
        await UniTask.CompletedTask;
    }
    public void AddCharacter(CharacterData character)
    {
        playerData.ownedCharacters.Add(new CharacterInstance(character));
    }

    public PurchaseResult TryPurchase(CharacterData character)
    {
        if (coins < character.price)
        {
            return new PurchaseResult(false, "Not enough coins");
        }

        playerData.coins -= character.price;
        OwnedCharacters.Add(new CharacterInstance(character));

        onCoinsChanged?.Invoke();
        onInventoryChange?.Invoke();

        return new PurchaseResult(true, "Purchase Successful");
    }
}
