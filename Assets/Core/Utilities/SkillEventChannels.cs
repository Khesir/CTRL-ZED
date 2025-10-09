using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Skill Event Channel")]
public class SkillEventChannel : ScriptableObject
{
    public Action<int, float> OnSkillUsed; // SkillIndex, cooldown
    public Action OnSkillsEquipped;
}
