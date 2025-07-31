using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InputService : MonoBehaviour, IInputService
{
    public static InputService Instance { get; private set; }

    // Main directional input
    public Vector2 MoveInput => new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    ).normalized;

    // Dash / Fire Inputs
    public bool DashPressed => Input.GetKeyDown(KeyCode.Space); // Or LeftShift?
    public bool IsDashPressed() => Input.GetKeyDown(KeyCode.LeftShift);
    public bool IsFirePressed() => Input.GetMouseButton(0);

    // Mouse World Position
    public Vector2 GetMouseWorldPosition()
    {
        if (Camera.main == null) return Vector2.zero;
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public bool SkillPressed(int slot)
    {
        return slot switch
        {
            0 => Input.GetKeyDown(KeyCode.Q),
            1 => Input.GetKeyDown(KeyCode.E),
            2 => Input.GetKeyDown(KeyCode.R), // optional third skill
            _ => false,
        };
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // avoid duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public async UniTask Initialize()
    {
        await UniTask.CompletedTask;
    }
}
