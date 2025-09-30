using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] public event Action onInventoryChange;
    public List<CharacterConfig> characterTemplates;
    public List<CharacterData> ownedCharacters;
    public async UniTask Initialize(List<CharacterData> characters)
    {
        if (ownedCharacters == null)
            ownedCharacters = new List<CharacterData>();

        if (characters != null && characters.Count > 0)
            ownedCharacters.AddRange(characters); // keep existing characters

        Debug.Log("[CharacterManager] Generating Character Templates");
        if (characterTemplates.Count == 0)
            characterTemplates.AddRange(Resources.LoadAll<CharacterConfig>("Characters"));

        Debug.Log("[CharacterManager] Loading saved characters");
        Debug.Log("[CharacterManager] Character Manager Initialized");
        await UniTask.CompletedTask;
    }
    public List<CharacterData> GetCharacters()
    {
        return ownedCharacters;
    }

    public void CreateCharacter(CharacterConfig instance = null)
    {
        if (instance == null) return;
        var character = CharacterFactory.CreateFromShop(instance);

        ownedCharacters.Add(character);
        onInventoryChange?.Invoke();
        Debug.Log("Created Character Services");
    }
}