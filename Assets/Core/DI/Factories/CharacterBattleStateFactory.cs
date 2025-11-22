using System.Collections.Generic;

/// <summary>
/// Factory for creating CharacterBattleState instances.
/// </summary>
public class CharacterBattleStateFactory
{
    public CharacterBattleState Create(CharacterData data)
    {
        var service = new CharacterService(data);
        return new CharacterBattleState(service);
    }

    public List<CharacterBattleState> CreateFromTeam(List<CharacterData> members)
    {
        var battleStates = new List<CharacterBattleState>();

        foreach (var member in members)
        {
            battleStates.Add(Create(member));
        }

        return battleStates;
    }
}
