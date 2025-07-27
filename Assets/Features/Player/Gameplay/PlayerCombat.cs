using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private readonly IInputService input;
    private readonly IWeapon weapon;
    [Header("Player Properties")]
    [SerializeField] private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;

    public PlayerCombat(IInputService input, IWeapon weapon)
    {
        this.input = input;
        this.weapon = weapon;
    }

    public void HandleCombat()
    {
        if (input.IsFirePressed())
        {
            weapon.Fire();
        }
    }
    public void TakeDamage(int rawDamage, GameObject source = null)
    {
        // int finalDamage = Mathf.Max(0, rawDamage - statService.GetDefense());
        // healthService.ReduceHealth(finalDamage);
        // effectService.PlayHitEffect();

        // Debug.Log($"Took {finalDamage} damage from {source?.name ?? "unknown"}");
    }
}
