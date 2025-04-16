using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int coins = 0;
    public List<CharacterInstance> ownedCharacters = new();

    public void AddCharacter(CharacterData character)
    {
        ownedCharacters.Add(new CharacterInstance(character));
    }

    public void SpendCoins(int amount)
    {
        coins -= amount;
        coins = Mathf.Max(coins, 0); // no negatives
    }

    public void EarnCoins(int amount)
    {
        coins += amount;
    }

    public int GetCharacterCount(CharacterData character)
    {
        return ownedCharacters.FindAll(c => c.template == character).Count;
    }
}
