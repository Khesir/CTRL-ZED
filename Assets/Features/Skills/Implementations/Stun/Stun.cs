using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour, ISkill, IStatProvider
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
    public float stunRadius = 5f;
    public float slowAmount = 0f;
    private CircleCollider2D triggerCollider;
    public List<EnemyService> enemiesInRange;
    [SerializeField] private Collider2D[] hits;
    private void Awake()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        triggerCollider.isTrigger = true;
        triggerCollider.radius = stunRadius;
    }
    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
    }
    public void Activate()
    {
        if (!CanActivate()) return;
        isActivate = true;
        lastUsedTime = Time.time;
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            // Match size to slow radius
            float scale = stunRadius * 2f; // diameter = radius * 2
            vfx.transform.localScale = new Vector3(scale, scale, 1f);
        }

        // Get all enemy within the stunRadius
        hits = Physics2D.OverlapCircleAll(transform.position, stunRadius, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            var enemyService = hit.GetComponent<EnemyService>();
            if (enemyService != null && !enemiesInRange.Contains(enemyService))
            {
                enemiesInRange.Add(enemyService);
                hit.GetComponent<EnemyFollow>().ApplyStun(slowAmount);
            }
        }

        Debug.Log($"{user.name} used {config.skillName}");
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime());
    }

    private IEnumerator RemoveAfterLifetime()
    {
        yield return new WaitForSeconds(config.skillLifetime);
        Destroy(vfx);
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            var enemy = enemiesInRange[i];
            if (enemy == null) // Enemy got destroyed/killed
            {
                enemiesInRange.RemoveAt(i);
                continue;
            }

            var follow = enemy.GetComponent<EnemyFollow>();
            if (follow != null)
                follow.ClearStun(); // or ClearSlow()

            enemiesInRange.RemoveAt(i); // cleanup
        }
        Debug.Log($"{user.name} {config.skillName} expired");
    }
    public bool CanActivate()
    {
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
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
}
