using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// State for active gameplay - waves, combat, etc.
/// </summary>
public class GameplayPlayingState : GameplayStateBase
{
    public GameplayPlayingState(GameplayManager manager) : base(manager) { }

    public override async UniTask Enter()
    {
        Debug.Log("[GameplayState] Entering Playing State");

        Manager.HandleTeamChangeInternal();

        if (Manager.GameplayUI != null)
        {
            await Manager.GameplayUI.PlayingStateUIAnimation();
        }

        Manager.StartWaveInternal();
    }

    public override void Update()
    {
        // Gameplay tick handled elsewhere
    }
}
