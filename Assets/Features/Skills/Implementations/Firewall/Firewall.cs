using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewall : MonoBehaviour, ISkill, IStatProvider
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;

    [Header("Modifier")]
    [SerializeField] private List<StatModifierData> stats;

    // Firewall settings
    public float range = 20f;         // Controls the size of the area
    public float attackTick = 2f;     // How often damage is applied
    public float damage = 10f;
    public float slowAmount = 0.3f;

    // Internal References
    private Animator vfxAnimator;
    private GameObject vfx;
    private CircleCollider2D triggerCollider;

    public List<EnemyService> enemiesInRange = new List<EnemyService>();
    private float counter;
    private bool activated = false;
    private Vector3 targetSize;

    private void Awake()
    {
        triggerCollider = GetComponent<CircleCollider2D>();
        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        // Always keep collider as unit circle so transform scale controls its real size
        triggerCollider.isTrigger = true;
        triggerCollider.radius = 0.5f;
    }

    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
        activated = false;
    }
    public void Activate()
    {
        if (!CanActivate()) return;

        activated = true;
        targetSize = Vector3.one * range;
        transform.localScale = Vector3.zero;

        // Spawn VFX and scale it to match range
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            vfx.transform.localScale = targetSize * 2f; // diameter scale
        }

        // Get all enemies in range instantly on cast
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Enemy"));
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
        user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveAfterLifetime());
    }
    private IEnumerator RemoveAfterLifetime()
    {
        yield return new WaitForSeconds(config.skillLifetime);
        activated = false;
        Destroy(vfx);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!activated) return;

        // Smooth grow to target size
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime);

        // Periodic damage
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = attackTick;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].TakeDamage(damage);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyService>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
                collision.GetComponent<EnemyFollow>().ApplySlow(slowAmount);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<EnemyService>();
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
                collision.GetComponent<EnemyFollow>().ClearSlow();
            }
        }
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
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public SkillConfig GetSkillConfig() => config;

}
