using Cysharp.Threading.Tasks;

/// <summary>
/// Base class for gameplay states with reference to GameplayManager.
/// </summary>
public abstract class GameplayStateBase : IState
{
    protected readonly GameplayManager Manager;

    protected GameplayStateBase(GameplayManager manager)
    {
        Manager = manager;
    }

    public abstract UniTask Enter();
    public virtual void Update() { }
    public virtual UniTask Exit() => UniTask.CompletedTask;
}
