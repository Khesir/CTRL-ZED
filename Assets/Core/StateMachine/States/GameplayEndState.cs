using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// State for gameplay end - victory or defeat.
/// </summary>
public class GameplayEndState : GameplayStateBase
{
    public GameplayEndState(GameplayManager manager) : base(manager) { }

    public override UniTask Enter()
    {
        Debug.Log("[GameplayState] Entering End State");

        Manager.EndGameplayInternal();
        return UniTask.CompletedTask;
    }
}
