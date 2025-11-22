using Cysharp.Threading.Tasks;

/// <summary>
/// Interface for state pattern implementation.
/// Each state encapsulates its own Enter/Update/Exit behavior.
/// </summary>
public interface IState
{
    UniTask Enter();
    void Update();
    UniTask Exit();
}

public interface IStateMachine
{
    void ChangeState(IState newState);
    void Update();
    IState CurrentState { get; }
}
