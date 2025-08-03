using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    private ISkill[] skills = new ISkill[2];
    public GameObject skillUser;

    public void EquipSkills(SkillConfig skill1, SkillConfig skill2)
    {
        EquipSkill(0, skill1);
        EquipSkill(1, skill2);
    }
    private void EquipSkill(int index, SkillConfig config)
    {
        if (config == null || config.skillPrefab == null) return;
        GameObject skillGO = Instantiate(config.skillPrefab, transform);
        skillGO.transform.localPosition = Vector3.zero;
        ISkill skill = skillGO.GetComponent<ISkill>();

        if (skill != null)
        {
            skill.Initialize(config, skillUser);
            skills[index] = skill;
        }
        else
        {
            Debug.LogError($"Skill prefab {config.skillPrefab.name} does not implement ISkill");
        }
    }
    public void UseSkill(int index)
    {
        if (index < 0 || index >= skills.Length) return;
        if (skills[index] != null && skills[index].CanActivate())
        {
            skills[index].Activate();
        }
    }
}
