using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// State for player death/revive handling.
/// </summary>
public class GameplayReviveState : GameplayStateBase
{
    public GameplayReviveState(GameplayManager manager) : base(manager) { }

    public override UniTask Enter()
    {
        Debug.Log("[GameplayState] Entering Revive State");

        Manager.PauseGameplayInternal();
        return UniTask.CompletedTask;
    }

    public override UniTask Exit()
    {
        Manager.ResumeGameplayInternal();
        return UniTask.CompletedTask;
    }
}
