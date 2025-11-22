using System.Collections.Generic;

/// <summary>
/// Manages command execution with support for queuing, processing, and undo.
/// </summary>
public class CommandQueue
{
    private readonly Queue<ICommand> _pendingCommands = new();
    private readonly Stack<ICommand> _executedCommands = new();
    private readonly int _maxHistorySize;

    public CommandQueue(int maxHistorySize = 100)
    {
        _maxHistorySize = maxHistorySize;
    }

    public int PendingCount => _pendingCommands.Count;
    public int HistoryCount => _executedCommands.Count;

    public void Enqueue(ICommand command)
    {
        if (command != null && command.CanExecute())
        {
            _pendingCommands.Enqueue(command);
        }
    }

    public void ExecuteImmediate(ICommand command)
    {
        if (command != null && command.CanExecute())
        {
            command.Execute();
            PushToHistory(command);
        }
    }

    public void ProcessNext()
    {
        if (_pendingCommands.Count == 0) return;

        var command = _pendingCommands.Dequeue();
        if (command.CanExecute())
        {
            command.Execute();
            PushToHistory(command);
        }
    }

    public void ProcessAll()
    {
        while (_pendingCommands.Count > 0)
        {
            ProcessNext();
        }
    }

    public void Undo()
    {
        if (_executedCommands.Count == 0) return;
        _executedCommands.Pop().Undo();
    }

    public void Clear()
    {
        _pendingCommands.Clear();
    }

    public void ClearHistory()
    {
        _executedCommands.Clear();
    }

    private void PushToHistory(ICommand command)
    {
        _executedCommands.Push(command);

        // Limit history size
        if (_executedCommands.Count > _maxHistorySize)
        {
            var temp = new Stack<ICommand>();
            for (int i = 0; i < _maxHistorySize; i++)
            {
                temp.Push(_executedCommands.Pop());
            }
            _executedCommands.Clear();
            while (temp.Count > 0)
            {
                _executedCommands.Push(temp.Pop());
            }
        }
    }
}
