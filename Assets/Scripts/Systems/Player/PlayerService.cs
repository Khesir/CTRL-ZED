using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService
{
    private PlayerData _data;

    public PlayerService(PlayerData data)
    {
        _data = data;
    }
    public PlayerData GetData()
    {
        return _data;
    }
    public bool HasCharacter(CharacterData instance) =>
        _data.ownedCharacters.Contains(instance);
    public int GetPlayerCoins()
    {
        return _data.coins;
    }
    public void AddCharacter(CharacterData character)
    {
        _data.ownedCharacters.Add(character);
    }
    public List<CharacterData> GetOwnedCharacters()
    {
        return _data.ownedCharacters;
    }
    public PurchaseResult PurchaseCharacter(CharacterConfig character)
    {

        if (_data.coins < character.price)
        {
            return new PurchaseResult(false, "Not enough coins");
        }

        _data.coins -= character.price;
        _data.ownedCharacters.Add(new CharacterData(character));

        return new PurchaseResult(true, "Purchase succesfull");
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