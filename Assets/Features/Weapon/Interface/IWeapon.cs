using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SourceType
{
    Enemy,
    Player,
}
public interface IWeapon
{
    void Fire();
    void Initialize(WeaponConfig weaponConfig, GameObject holder, SourceType type);

    // Temporary Add
    public void UpdateFirerate(float rate);
    public float GetFirerate();
}
