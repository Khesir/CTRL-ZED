using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BirdsEye : MonoBehaviour, ISkill, IStatProvider
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;


    [Header("Modifier")]
    [SerializeField] private List<StatModifierData> stats;


    [Header("Dash Settings")]
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashSpeed = 15f;

    // Internally referenced and controlled
    private Animator vfxAnimator;
    private GameObject vfx;


    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;

    }
    public void Activate()
    {
        if (!CanActivate()) return;

        lastUsedTime = Time.time;

        // Skill Logic
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            vfx.transform.localScale = new Vector3(2f, 2f, 1f);
            Destroy(vfx, vfx.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
        var moveInput = gameObject.transform.right;
        if (InputService.Instance.MoveInput != Vector2.zero)
            moveInput = InputService.Instance.MoveInput;
        // Temporary solution:  Currently Tightly couple to one of service (which is bad) -- Temp solution
        transform.parent.gameObject
            .transform.parent.gameObject.GetComponent<PlayerDash>()
            .HandleDashInput(true, moveInput, true, 3);

        Debug.Log($"{user.name} used {config.skillName}");
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime());

    }
    private IEnumerator RemoveAfterLifetime()
    {
        yield return new WaitForSeconds(config.skillLifetime);
        Debug.Log($"{user.name} {config.skillName} expired");

    }
    public bool CanActivate()
    {
        if (lastUsedTime == 0) return true;
        return Time.time >= lastUsedTime + config.cooldown;
    }
    public float CooldownRemaining => Math.Max(0, (lastUsedTime + config.cooldown) - Time.time);
    public float CooldPercent => 1f - (CooldownRemaining / config.cooldown);
    public IEnumerable<StatModifier> GetModifiers()
    {
        foreach (var mod in stats)
        {
            if (mod.value != 0)
            {
                yield return new StatModifier(mod.statId, mod.value, mod.type, mod.priority, this);
            }
        }
    }
    public SkillConfig GetSkillConfig() => config;

}
