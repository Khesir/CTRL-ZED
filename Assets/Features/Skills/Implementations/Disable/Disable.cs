using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour, ISkill, IStatProvider
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
    private bool isActivate = false;
    [Header("Area Settings")]
    public float slowRadius = 5f;
    public float slowAmount = 0.3f;
    private CircleCollider2D triggerCollider;
    public List<EnemyService> enemiesInRange;

    private void Awake()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        triggerCollider.isTrigger = true;
        triggerCollider.radius = slowRadius;
    }
    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
        slowAmount = 0;
    }
    public void Activate()
    {
        if (!CanActivate()) return;
        isActivate = true;
        lastUsedTime = Time.time;
        var state = user.GetComponent<PlayerGameplayService>()?.GetCharacterState();
        state.isDisabled = true;
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            // Match size to slow radius
            float scale = slowRadius * 2f; // diameter = radius * 2
            vfx.transform.localScale = new Vector3(scale, scale, 1f);
        }
        var sr = user.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }
        // Get all enemy within the slowradius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, slowRadius, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            var enemyService = hit.GetComponent<EnemyService>();
            if (enemyService != null && !enemiesInRange.Contains(enemyService))
            {
                enemiesInRange.Add(enemyService);
                hit.GetComponent<EnemyFollow>().ApplySlow(slowAmount);
            }
        }

        Debug.Log($"{user.name} used {config.skillName}");
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime(state));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivate) return;
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Add(collision.GetComponent<EnemyService>());
            collision.GetComponent<EnemyFollow>().ApplyStun(slowAmount);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isActivate) return;

        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.GetComponent<EnemyService>());
            collision.GetComponent<EnemyFollow>().ClearStun();
        }
    }
    private IEnumerator RemoveAfterLifetime(CharacterBattleState state)
    {
        yield return new WaitForSeconds(config.skillLifetime);
        Destroy(vfx);
        state.isDisabled = false;
        isActivate = false;
        var sr = user.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
        Debug.Log($"{user.name} {config.skillName} expired");
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
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
    public SkillConfig GetSkillConfig() => config;

}
