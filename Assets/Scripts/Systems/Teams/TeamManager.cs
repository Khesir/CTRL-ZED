using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [HideInInspector] private TeamService service;
    public async UniTask Initialize(PlayerData playerData)
    {
        service = new TeamService(playerData);
        // Just for testing
        service.CreateTeam();
        service.CreateTeam();
        await UniTask.CompletedTask;
    }
    public List<Team> GetTeams()
    {
        return service.GetTeams();
    }
    public Response<object> AssignedCharacterToSlot(int teamIndex, int slotIndex, CharacterData character)
    {
        return service.AssignedCharacterToSlot(teamIndex, slotIndex, character);
    }
    public Response<object> RemoveCharacterFromSlot(int teamIndex, int slotIndex)
    {
        return service.RemoveCharacterFromSlot(teamIndex, slotIndex);
    }
    public Response<object> isCharacterInTeam(int teamIndex, CharacterData character)
    {
        return service.isCharacterInTeam(teamIndex, character);
    }
}
