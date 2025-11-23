using System;
using Cysharp.Threading.Tasks;

public interface IGameplayManager
{
    // State
    bool IsGameActive { get; }
    GameplayState CurrentState { get; }
    GameplayEndGameState EndGameState { get; set; }

    // Team Management
    string ActiveTeamID { get; set; }
    event Action OnDeadTeamUpdated;
    bool IsTeamDead(string teamID);
    void SetDeadTeam(string teamID, bool isDead);

    // State Transitions
    UniTask SetState(GameplayState newState);
    void TriggerEndGame();
}
