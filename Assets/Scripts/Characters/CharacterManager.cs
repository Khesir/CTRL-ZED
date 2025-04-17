using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterData[] characterTemplates;

    public async UniTask Initialize()
    {
        foreach (var template in characterTemplates)
        {

            GameManager.Instance.PlayerManager.AddCharacter(template);

            Debug.Log("Generated Character: " + template.charactername);
        }
        await UniTask.CompletedTask;
    }
    // void GenerateCharacterData()
    // {
    //     foreach (var template in characterTemplates)
    //     {
    //         if (GameManager.Instance != null && GameManager.Instance.playerData != null)
    //         {

    //             GameManager.Instance.playerData.AddCharacter(template);

    //             Debug.Log("Generated Character: " + template.charactername);
    //         }
    //     }
    // }

    // I can later create a gameobject to generate it as gameObject
}


