using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface ICharacterManager
{
    event Action onInventoryChange;
    List<CharacterConfig> characterTemplates { get; }
    List<CharacterData> ownedCharacters { get; }

    UniTask Initialize(List<CharacterData> characters);
    List<CharacterData> GetCharacters();
    void CreateCharacter(CharacterConfig instance = null);
}
