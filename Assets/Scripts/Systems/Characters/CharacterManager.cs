using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<CharacterConfig> characterTemplates;

    public async UniTask Initialize()
    {
        foreach (var template in characterTemplates)
        {
            // Only for testing
            GameManager.Instance.PlayerManager.AddCharacter(new CharacterData(template));

            Debug.Log("Generated Character: " + template.charactername);
        }
        await UniTask.CompletedTask;
    }
    // Management of characters
}


