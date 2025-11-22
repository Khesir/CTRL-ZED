using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Generic state machine implementation with async Enter/Exit support.
/// </summary>
public class StateMachine : IStateMachine
{
    public IState CurrentState { get; private set; }
    private bool _isTransitioning;

    public void ChangeState(IState newState)
    {
        if (_isTransitioning || newState == null) return;
        ChangeStateAsync(newState).Forget();
    }

    private async UniTaskVoid ChangeStateAsync(IState newState)
    {
        _isTransitioning = true;

        if (CurrentState != null)
        {
            await CurrentState.Exit();
        }

        CurrentState = newState;
        await CurrentState.Enter();

        _isTransitioning = false;
    }

    public void Update()
    {
        if (!_isTransitioning)
        {
            CurrentState?.Update();
        }
    }
}
