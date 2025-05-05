using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService
{
    private PlayerData data;
    public PlayerService(PlayerData data)
    {
        this.data = data;
    }
    // public PlayerData GetData()
    // {
    //     return _data;
    // }
    // public bool HasCharacter(CharacterData instance) =>
    //     _data.ownedCharacters.Contains(instance);
    public int GetPlayerCoins()
    {
        return data.coins;
    }
    public bool SpendCoins(int amount)
    {
        if (amount <= data.coins)
        {
            data.coins -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}

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