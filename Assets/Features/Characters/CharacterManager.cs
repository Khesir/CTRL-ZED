using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<CharacterService> characters = new();
    [SerializeField] public event Action onInventoryChange;

    public async UniTask Initialize(List<CharacterData> characters)
    {
        foreach (var character in characters)
        {
            this.characters.Add(new CharacterService(character));
        }
        Debug.Log("[CharacterManager] Character Manager Initialized");
        await UniTask.CompletedTask;
    }
    public List<CharacterService> GetCharacters()
    {
        return characters;
    }

    public void CreateCharacter(CharacterConfig instance = null)
    {
        if (instance == null) return;
        var character = CharacterFactory.CreateFromShop(instance);

        characters.Add(character);

        onInventoryChange?.Invoke();
        Debug.Log("Created Character Services");
    }
}