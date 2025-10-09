using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : MonoBehaviour, ISkill, IStatProvider
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;

    [Header("Modifier")]
    [SerializeField] private List<StatModifierData> stats;


    // Internally referenced and controlled
    private Animator vfxAnimator;
    private GameObject vfx;

    public float increasedRate = 10;
    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
    }
    public void Activate()
    {
        if (!CanActivate()) return;
        var state = user.GetComponent<PlayerGameplayService>();
        lastUsedTime = Time.time;
        var prevRate = state.GetFirerate();
        state.UpdateFireRate(increasedRate);
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            vfx.transform.localScale = new Vector3(2f, 2f, 1f);
            Destroy(vfx, vfx.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime(prevRate, state));
    }
    private IEnumerator RemoveAfterLifetime(float prevRate, PlayerGameplayService state)
    {
        yield return new WaitForSeconds(config.skillLifetime);
        state.UpdateFireRate(prevRate);
    }
    public bool CanActivate()
    {
        if (lastUsedTime == 0) return true;
        return Time.time >= lastUsedTime + config.cooldown;
    }
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
