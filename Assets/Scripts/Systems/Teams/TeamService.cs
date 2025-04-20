using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamService
{
    private PlayerData playerData;

    public TeamService(PlayerData data)
    {
        playerData = data;
    }
    public List<Team> GetTeams() => playerData.teams;

    public Team GetTeam(int index)
    {
        if (index >= 0 && index < playerData.teams.Count)
        {
            return playerData.teams[index];
        }
        return null;
    }
}
