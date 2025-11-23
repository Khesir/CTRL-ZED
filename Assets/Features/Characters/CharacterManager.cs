using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour, ICharacterManager
{
    public event Action onInventoryChange;
    [SerializeField] private List<CharacterConfig> _characterTemplates;
    [SerializeField] private List<CharacterData> _ownedCharacters;

    public List<CharacterConfig> characterTemplates => _characterTemplates;
    public List<CharacterData> ownedCharacters => _ownedCharacters;
    public async UniTask Initialize(List<CharacterData> characters)
    {
        if (_ownedCharacters == null)
            _ownedCharacters = new List<CharacterData>();

        if (characters != null && characters.Count > 0)
            _ownedCharacters.AddRange(characters); // keep existing characters

        Debug.Log("[CharacterManager] Generating Character Templates");
        if (_characterTemplates == null)
            _characterTemplates = new List<CharacterConfig>();
        if (_characterTemplates.Count == 0)
            _characterTemplates.AddRange(Resources.LoadAll<CharacterConfig>("Characters"));

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

        _ownedCharacters.Add(character);
        onInventoryChange?.Invoke();
        Debug.Log("Created Character Services");
    }
}