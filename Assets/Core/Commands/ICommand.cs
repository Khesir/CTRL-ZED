/// <summary>
/// Command pattern interface for encapsulating actions.
/// Enables input buffering, undo/redo, and replay systems.
/// </summary>
public interface ICommand
{
    void Execute();
    void Undo();
    bool CanExecute();
}
