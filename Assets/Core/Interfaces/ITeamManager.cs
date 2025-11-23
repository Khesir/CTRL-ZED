using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface ITeamManager
{
    event Action onTeamChange;
    List<Team> ownedTeam { get; }
    int increaseSizePrice { get; }

    UniTask Initialize(List<Team> teams, int maxSize = 1, int increaseSizePrice = 10000);
    bool isTeamActive(string teamId);
    string CreateTeam();
    void IncreaseMaxTeam();
    List<TeamService> GetActiveTeam();
    void SetActiveTeam(string teamId);
    void RemoveActiveTeam(string teamId);
    List<TeamService> GetTeams();
    TeamService GetTeam(string index);
    Response<object> isCharacterInTeam(string teamId, CharacterData character);
    void AssignedCharacterToSlot(string teamId, int slotIndex, CharacterData character);
    Response<object> RemoveCharacterFromSlot(string teamId, int slotIndex);
    bool RemoveCharacterFromTeamByReference(string teamId, CharacterData character);
}
