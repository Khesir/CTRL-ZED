using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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

    public Response<object> CreateTeam()
    {
        if (playerData.currentMaxTeam > playerData.teams.Count)
        {
            var teamData = new Team("Team " + (playerData.teams.Count + 1));
            playerData.teams.Add(teamData);
            return Response.Success($"Successfully Created Team");
        }
        return Response.Fail($"Failed to Created Team");
    }
    public Response<object> AssignedCharacterToSlot(int teamIndex, int slotIndex, CharacterData character)
    {
        if (character == null || !playerData.ownedCharacters.Contains(character))
        {
            return Response.Fail("You don't own the character");
        }
        var team = GetTeam(teamIndex);
        if (team != null && slotIndex < team.characters.Count)
        {
            team.characters[slotIndex] = character;
            return Response.Success($"Assigned Character to Team {teamIndex} Slot {slotIndex}");
        }
        else
        {

            return Response.Fail("Position Exceeded");
        }
    }
    public Response<object> RemoveCharacterFromSlot(int teamIndex, int slotIndex)
    {
        var team = GetTeam(teamIndex);
        if (team != null && slotIndex < team.characters.Count)
        {
            if (team.characters[slotIndex] != null)
            {
                team.characters[slotIndex] = null;
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

    public Response<object> isCharacterInTeam(int teamIndex, CharacterData character)
    {
        var team = GetTeam(teamIndex);
        if (team.characters.Contains(character))
        {
            return Response.Fail("Character is team");
        }
        return Response.Success("Character Available");
    }
}
