using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// State for gameplay setup and initial animations.
/// </summary>
public class GameplayStartState : GameplayStateBase
{
    public GameplayStartState(GameplayManager manager) : base(manager) { }

    public override async UniTask Enter()
    {
        Debug.Log("[GameplayState] Entering Start State");

        if (!Manager.IsGameActive)
        {
            Manager.SetupLevelInternal();
        }

        if (Manager.GameplayUI != null)
        {
            await Manager.GameplayUI.StartStateUIAnimation();
        }

        Manager.SetGameActive(true);
    }

    public override void Update()
    {
        // Wait for player to start wave
    }
}
