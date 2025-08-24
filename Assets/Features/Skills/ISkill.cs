using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    void Initialize(SkillConfig config, GameObject go);
    void Activate();
    bool CanActivate();
    public SkillConfig GetSkillConfig();
}
