using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public List<CharacterConfig> characterTemplates;
    private CharacterService service;
    public async UniTask Initialize(PlayerManager manager)
    {
        service = new CharacterService();
        foreach (var template in characterTemplates)
        {
            // Only for testing
            manager.AddCharacter(service.CreateCharacter(template).Data);

            Debug.Log("Generated Character: " + template.charactername);
        }
        await UniTask.CompletedTask;
    }
    // Management of characters
    public Response<object> AssignToTeam(int teamIndex)
    {
        return service.AssigntoTeam(teamIndex);
    }
}


