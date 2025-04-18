using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData;

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

    public void SpendCoins(int amount)
    {
        playerData.coins -= amount;
        playerData.coins = Mathf.Max(playerData.coins, 0); // no negatives
    }

    public void EarnCoins(int amount)
    {
        playerData.coins += amount;
    }

    public int GetCharacterCount(CharacterData character)
    {
        return playerData.ownedCharacters.FindAll(c => c.template == character).Count;
    }
}
