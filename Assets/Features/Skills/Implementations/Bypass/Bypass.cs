using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bypass Skill - Performs a powerful dash that kills all enemies on contact
/// Allows the player to quickly eliminate enemies while repositioning
/// </summary>
public class Bypass : MonoBehaviour, ISkill
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;

    [Header("Bypass Settings")]
    [Tooltip("Speed multiplier for the bypass dash (higher = faster)")]
    public float bypassDashMultiplier = 3f;

    [Tooltip("Collision radius for detecting enemies during dash")]
    public float collisionRadius = 1.5f;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject vfx;
    private AudioSource audioSource;

    // Internal state
    private PlayerDash playerDash;
    private PlayerGameplayService playerService;
    private GameObject bypassHitbox;
    private CircleCollider2D bypassCollider;
    private List<EnemyService> hitEnemies = new List<EnemyService>();
    private bool isDashingBypass = false;
    private float dashEndTime;

    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
        audioSource = GetComponent<AudioSource>();

        // Get player components
        playerDash = user.GetComponent<PlayerDash>();
        playerService = user.GetComponent<PlayerGameplayService>();

        if (playerDash == null)
        {
            Debug.LogError("[Bypass] PlayerDash component not found on user!");
        }

        // Create a child GameObject with trigger collider for enemy detection
        // This prevents interference with player's existing colliders
        bypassHitbox = new GameObject("BypassHitbox");
        bypassHitbox.transform.SetParent(user.transform);
        bypassHitbox.transform.localPosition = Vector3.zero;
        bypassHitbox.layer = user.layer;

        bypassCollider = bypassHitbox.AddComponent<CircleCollider2D>();
        bypassCollider.isTrigger = true;
        bypassCollider.radius = collisionRadius;

        // Add this script as component to handle trigger events
        BypassTriggerHandler handler = bypassHitbox.AddComponent<BypassTriggerHandler>();
        handler.Initialize(this);

        bypassHitbox.SetActive(false); // Only enable during bypass dash
    }

    public void Activate()
    {
        if (!CanActivate())
        {
            Debug.Log("[Bypass] Skill on cooldown!");
            return;
        }

        if (playerDash == null || playerService == null)
        {
            Debug.LogError("[Bypass] Required components not found!");
            return;
        }

        lastUsedTime = Time.time;

        // Get movement input direction
        Vector2 moveInput = InputService.Instance != null ? InputService.Instance.MoveInput : Vector2.zero;
        if (moveInput.magnitude < 0.1f)
        {
            // Default to facing direction or last move direction
            moveInput = Vector2.right; // Default direction if no input
        }

        // Start the bypass dash
        float dashDuration = config.skillLifetime > 0 ? config.skillLifetime : 0.3f;
        dashEndTime = Time.time + dashDuration;
        isDashingBypass = true;
        hitEnemies.Clear();

        // Trigger dash with override
        playerDash.HandleDashInput(true, moveInput, true, bypassDashMultiplier);

        // Enable collision detection
        if (bypassHitbox != null)
        {
            bypassHitbox.SetActive(true);
        }

        // Spawn visual effect
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity, user.transform);
            Destroy(vfx, dashDuration);
        }

        // Play sound effect
        if (audioSource != null)
        {
            audioSource.Play();
        }

        Debug.Log($"[Bypass] Dash initiated! Duration: {dashDuration}s, Multiplier: {bypassDashMultiplier}x");

        // Schedule cleanup
        StartCoroutine(EndBypassAfterDuration(dashDuration));
    }

    private void Update()
    {
        // Check if bypass dash has ended
        if (isDashingBypass && Time.time >= dashEndTime)
        {
            EndBypass();
        }
    }

    /// <summary>
    /// Called by BypassTriggerHandler when an enemy is hit
    /// </summary>
    public void OnEnemyHit(EnemyService enemy)
    {
        // Only process collisions during bypass dash
        if (!isDashingBypass) return;

        if (enemy != null && !hitEnemies.Contains(enemy))
        {
            // Kill the enemy instantly
            hitEnemies.Add(enemy);
            enemy.SilentKill(); // Use silent kill to avoid normal death effects
            Debug.Log($"[Bypass] Eliminated enemy: {enemy.gameObject.name}");
        }
    }

    private IEnumerator EndBypassAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndBypass();
    }

    private void EndBypass()
    {
        if (!isDashingBypass) return;

        isDashingBypass = false;

        // Disable collision detection
        if (bypassHitbox != null)
        {
            bypassHitbox.SetActive(false);
        }

        Debug.Log($"[Bypass] Dash ended. Enemies eliminated: {hitEnemies.Count}");
    }

    public bool CanActivate()
    {
        if (lastUsedTime == 0) return true;
        return Time.time >= lastUsedTime + config.cooldown;
    }

    public SkillConfig GetSkillConfig()
    {
        return config;
    }

    private void OnDestroy()
    {
        // Clean up
        if (vfx != null)
        {
            Destroy(vfx);
        }

        if (bypassHitbox != null)
        {
            Destroy(bypassHitbox);
        }
    }
}

/// <summary>
/// Helper component to handle trigger collisions for Bypass skill
/// Attached to the bypass hitbox child GameObject
/// </summary>
public class BypassTriggerHandler : MonoBehaviour
{
    private Bypass bypassSkill;

    public void Initialize(Bypass skill)
    {
        bypassSkill = skill;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bypassSkill == null) return;

        // Check if we hit an enemy
        EnemyService enemy = collision.GetComponent<EnemyService>();
        if (enemy != null)
        {
            bypassSkill.OnEnemyHit(enemy);
        }
    }
}
