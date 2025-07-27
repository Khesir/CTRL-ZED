using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputService
{
    Vector2 MoveInput { get; }
    bool DashPressed { get; }
    bool IsFirePressed();
    Vector2 GetMouseWorldPosition();
    bool IsDashPressed();
    bool SkillPressed(int slot);
}
