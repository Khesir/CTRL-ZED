using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, ISkill, IStatProvider
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;
    [Header("Modifier")]
    [SerializeField] private List<StatModifierData> stats;
    // Kinda fix but extendable for block skill
    public float pushRadius = 10f;   // configurable
    public float pushForce = 50f;   // configurable

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
        var state = user.GetComponent<PlayerGameplayService>()?.GetCharacterState();
        state.AddStatProvider(this);

        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            vfxAnimator = vfx.GetComponent<Animator>(); // play start animation
        }
        Debug.Log($"{user.name} used {config.skillName}");
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime(state));
    }
    private IEnumerator RemoveAfterLifetime(CharacterBattleState state)
    {
        vfxAnimator.SetTrigger("Trigger"); // play active animation by trigger once
        yield return new WaitForSeconds(config.skillLifetime);
        Destroy(vfx);
        state.RemoveStatProvider(this);
        Debug.Log($"{user.name} {config.skillName} expired");
        vfxAnimator.SetTrigger("Trigger"); // play close animation by trigger once

        yield return new WaitForSeconds(0.2f); // Wait for animator to wind-up
        var sr = user.GetComponent<SpriteRenderer>();
        StartCoroutine(FlashSprite(sr));
        Collider2D[] hits = Physics2D.OverlapCircleAll(user.transform.position, pushRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Rigidbody2D rb = hit.attachedRigidbody;
                if (rb != null)
                {
                    Vector2 pushDir = (hit.transform.position - user.transform.position).normalized;
                    rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
                    Debug.Log($"Pushed {hit.name} away");
                }
            }
        }
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
    private IEnumerator FlashSprite(SpriteRenderer sr)
    {
        Color original = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(1f);
        sr.color = original;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRadius);

        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, pushForce);
    }
    public SkillConfig GetSkillConfig() => config;

}
