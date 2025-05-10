using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponLevel;
    public List<WeaponStats> stats;
}
[System.Serializable]
public class WeaponStats
{
    public float cooldown;
    public float duration;
    public float damage;
    public float range;
    public float speed;
    public float slowRate;
}