/// <summary>
/// Command for firing a weapon.
/// </summary>
public class FireCommand : ICommand
{
    private readonly WeaponHolder _weaponHolder;

    public FireCommand(WeaponHolder weaponHolder)
    {
        _weaponHolder = weaponHolder;
    }

    public bool CanExecute()
    {
        return _weaponHolder != null;
    }

    public void Execute()
    {
        _weaponHolder.Fire();
    }

    public void Undo()
    {
        // Firing is not undoable
    }
}
