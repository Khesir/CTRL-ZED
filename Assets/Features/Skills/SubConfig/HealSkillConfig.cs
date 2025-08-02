using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Skills/HealSkill")]
public class HealSkillConfig : SkillConfig
{
    [Header("Heal Properties")]
    public float healAmount = 20f;
}
