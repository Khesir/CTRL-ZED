using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private List<TeamService> teams = new();
    [SerializeField] private TeamService activeTeam;
    [SerializeField] private int maxSize;
    public event Action onTeamChange;

    public async UniTask Initialize(List<Team> teams, int maxSize = 1)
    {
        this.maxSize = maxSize;
        foreach (var team in teams)
        {
            this.teams.Add(new TeamService(team));
        }
        await UniTask.CompletedTask;
    }
    public void CreateTeam()
    {
        if (teams.Count < maxSize)
        {
            teams.Add(new TeamService());
            onTeamChange?.Invoke();
        }
        else
        {
            Debug.LogError("Max size met");
        }
    }
    public TeamService GetActiveTeam()
    {
        return activeTeam;
    }
    public void SetActiveTeam(int index)
    {
        if (index >= 0 && index < teams.Count)
        {
            activeTeam = teams[index];
            Debug.Log($"{index} Team set as active team");
        }
        else
        {
            Debug.Log("Index out of scope");
        }
    }
    public List<TeamService> GetTeams()
    {
        return teams;
    }
    public TeamService GetTeam(int index)
    {
        if (index >= 0 && index < teams.Count)
        {
            return teams[index];
        }
        return null;
    }
    public Response<object> isCharacterInTeam(int teamIndex, CharacterService character)
    {
        var team = GetTeam(teamIndex);
        if (team.GetMembers().Contains(character))
        {
            return Response.Fail("Character is team");
        }
        return Response.Success("Character Available");
    }
    public void AssignedCharacterToSlot(int teamIndex, int slotIndex, CharacterService character)
    {
        if (character == null || GameManager.Instance.CharacterManager.GetCharacters().Contains(character))
        {
            Debug.LogError("You don't own the character");
        }
        var team = GetTeam(teamIndex);
        if (team != null && slotIndex < team.GetMembers().Count)
        {
            team.GetMembers()[slotIndex] = character;
            Debug.Log($"Assigned Character to Team {teamIndex} Slot {slotIndex}");
        }
        else
        {

            Debug.Log("Position Exceeded");
        }
    }
    public Response<object> RemoveCharacterFromSlot(int teamIndex, int slotIndex)
    {
        var team = GetTeam(teamIndex);
        if (team != null && slotIndex < team.GetMembers().Count)
        {
            if (team.GetMembers()[slotIndex] != null)
            {
                team.GetMembers()[slotIndex] = null;
                return Response.Success($"Removed character from Team {teamIndex} Slot {slotIndex}");
            }
            else
            {
                return Response.Fail("No character to remove in this slot");
            }
        }
        else
        {
            return Response.Fail("Something went wrong clearly");
        }
    }
}