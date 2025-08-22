using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Skill")]
public class SkillConfig : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite icon;
    public float cooldown;
    public float skillLifetime = 2f;

    [Tooltip("Must have a component that implements ISkill")]
    public GameObject skillPrefab;

    [Header("Visuals")]
    public GameObject vfxPrefab;
}
