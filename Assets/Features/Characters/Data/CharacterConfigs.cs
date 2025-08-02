using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Characters/Character")]
public class CharacterConfig : ScriptableObject
{
    public Sprite icon;
    public string className;
    public string charactername;
    public Sprite ship;

    [Header("Stats")]
    public int baseAttack = 1;
    public int baseHealth;
    public int defense;
    public int dex;
    public int level;
    // Movement
    [Header("Movement")]
    public float moveSpeed = 5f;
    [Header("Dash Controls")]
    public float dashCooldown = 1f;
    public float dashDuration = 0.2f;
    public float dashSpeed = 10f;

    [Header("Shop Info")]
    public int price;
    [Header("Deployment Cost")]
    public float food;
    public float technology;
    public float energy;
    public float intelligence;

    [Header("Weapon")]
    public WeaponConfig weapon;

    [Header("Skill")]
    public SkillConfig skill1;
    public SkillConfig skill2;
}
