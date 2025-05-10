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
    public int increaseSizePrice;
    public event Action onTeamChange;

    public async UniTask Initialize(List<Team> teams, int maxSize = 1, int increaseSizePrice = 10000)
    {
        this.maxSize = maxSize;
        this.increaseSizePrice = increaseSizePrice;
        foreach (var team in teams)
        {
            this.teams.Add(new TeamService(team));
        }
        await UniTask.CompletedTask;
    }
    public bool IsTeamActiveByIndex(int index)
    {
        return teams[index].isActive;
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
    public void IncreaseMaxTeam()
    {
        maxSize++;
    }
    public TeamService GetActiveTeam()
    {
        return activeTeam;
    }
    public void SetActiveTeam(int index)
    {
        if (index >= 0 && index < teams.Count)
        {
            if (activeTeam != null)
            {
                activeTeam.isActive = false;
            }
            activeTeam = teams[index];
            activeTeam.isActive = true;
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
        if (character == null || !GameManager.Instance.CharacterManager.GetCharacters().Contains(character))
        {
            Debug.LogError("You don't own the character");
            return;
        }
        var team = GetTeam(teamIndex);
        if (team != null && slotIndex < team.GetMembers().Count)
        {
            team.GetMembers()[slotIndex] = character;
            Debug.Log($"Assigned Character to Team {teamIndex} Slot {slotIndex}");
            return;
        }
        else
        {
            Debug.Log("Position Exceeded");
            return;
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
    public bool RemoveCharacterFromTeamByReference(int teamIndex, CharacterService character)
    {
        var team = GetTeam(teamIndex);
        if (team == null)
        {
            return false;
        }

        var members = team.GetMembers();
        int index = members.IndexOf(character);
        if (index != -1)
        {
            members[index] = null;
            onTeamChange?.Invoke();
            return true;
        }

        return false;
    }
}