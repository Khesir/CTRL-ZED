using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private List<TeamService> teams = new();
    [SerializeField] private List<TeamService> activeTeam = new();
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
        Debug.Log("[TeamManager] Player Manager Initialized");
        await UniTask.CompletedTask;
    }
    public bool isTeamActive(string teadID)
    {
        if (activeTeam.Count < 1)
            return false;
        foreach (TeamService team in activeTeam)
        {
            if (team.teamID == teadID)
            {
                return true;
            }
        }
        return false;
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
        Debug.Log(teams.Count);
    }
    public void IncreaseMaxTeam()
    {
        maxSize++;
    }
    public List<TeamService> GetActiveTeam()
    {
        foreach (var team in activeTeam)
        {
            Debug.Log($"Active Team: {team.teamID}");
        }
        return activeTeam;
    }
    public void SetActiveTeam(string teadID)
    {
        var selectedTeam = GetTeam(teadID);
        foreach (TeamService team in activeTeam)
        {
            if (team.teamID == selectedTeam.teamID)
            {
                Debug.Log("This team is already in the active list");
                return;
            }
        }
        activeTeam.Add(selectedTeam);
        onTeamChange?.Invoke();
        Debug.Log($"{selectedTeam.teamID} Team set as active team");
    }
    public void RemoveActiveTeam(string teamId)
    {
        var selectedTeam = GetTeam(teamId);
        var exists = false;
        foreach (TeamService team in activeTeam)
        {
            if (team.teamID == selectedTeam.teamID)
            {
                exists = true;
            }
        }
        if (exists)
        {
            activeTeam.Remove(selectedTeam);
            onTeamChange?.Invoke();
            Debug.Log($"{selectedTeam.teamID} Team set as active team");
        }
    }
    public List<TeamService> GetTeams()
    {
        return teams;
    }
    public TeamService GetTeam(string index)
    {
        foreach (TeamService team in teams)
        {
            if (team.teamID == index)
            {
                return team;
            }
        }
        return null;
    }
    public Response<object> isCharacterInTeam(string teamId, CharacterService character)
    {
        var team = GetTeam(teamId);
        if (team.GetMembers().Contains(character))
        {
            return Response.Fail("Character is team");
        }
        return Response.Success("Character Available");
    }
    public void AssignedCharacterToSlot(string teamId, int slotIndex, CharacterService character)
    {
        if (character == null || !GameManager.Instance.CharacterManager.GetCharacters().Contains(character))
        {
            Debug.LogError("You don't own the character");
            return;
        }
        var team = GetTeam(teamId);
        if (team != null && slotIndex < team.GetMembers().Count)
        {
            team.GetMembers()[slotIndex] = character;
            Debug.Log($"Assigned Character to Team {teamId} Slot {slotIndex}");
            return;
        }
        else
        {
            Debug.Log("Position Exceeded");
            return;
        }
    }
    public Response<object> RemoveCharacterFromSlot(string teamId, int slotIndex)
    {
        var team = GetTeam(teamId);
        if (team != null && slotIndex < team.GetMembers().Count)
        {
            if (team.GetMembers()[slotIndex] != null)
            {
                team.GetMembers()[slotIndex] = null;
                return Response.Success($"Removed character from Team {teamId} Slot {slotIndex}");
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
    public bool RemoveCharacterFromTeamByReference(string teamId, CharacterService character)
    {
        var team = GetTeam(teamId);
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