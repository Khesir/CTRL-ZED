using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<CharacterData> characterTemplates;

    public async UniTask Initialize()
    {
        foreach (var template in characterTemplates)
        {

            GameManager.Instance.PlayerManager.AddCharacter(template);

            Debug.Log("Generated Character: " + template.charactername);
        }
        await UniTask.CompletedTask;
    }
}


