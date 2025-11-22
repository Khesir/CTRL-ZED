using UnityEngine;

/// <summary>
/// Command for using a skill from the SkillHolder.
/// </summary>
public class UseSkillCommand : ICommand
{
    private readonly SkillHolder _skillHolder;
    private readonly int _skillIndex;

    public UseSkillCommand(SkillHolder skillHolder, int skillIndex)
    {
        _skillHolder = skillHolder;
        _skillIndex = skillIndex;
    }

    public bool CanExecute()
    {
        return _skillHolder != null;
    }

    public void Execute()
    {
        _skillHolder.UseSkill(_skillIndex);
    }

    public void Undo()
    {
        // Skills are typically not undoable
    }
}
