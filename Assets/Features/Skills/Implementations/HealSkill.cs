using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : MonoBehaviour, ISkill
{
    private HealSkillConfig config;
    private GameObject user;
    private float lastUsedTime;

    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config as HealSkillConfig;
        this.user = user;
    }


    public bool CanActivate()
    {
        return Time.time >= lastUsedTime + config.cooldown;
    }
    public void Activate()
    {
        if (!CanActivate()) return;

        lastUsedTime = Time.time;
        var state = user.GetComponent<PlayerGameplayService>()?.GetCharacterState();
        state?.Heal(config.healAmount);

        // VFX from config
        if (config.vfxPrefab != null)
        {
            GameObject vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity);
            Destroy(vfx, config.vfxLifetime);
        }

        Debug.Log($"{user.name} used {config.skillName}");
    }
}
