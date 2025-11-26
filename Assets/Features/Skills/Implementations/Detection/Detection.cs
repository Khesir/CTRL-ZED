using UnityEngine;
using Enemies;

/// <summary>
/// Detection Skill - Reveals all enemies on the minimap temporarily
/// Allows the player to see enemy positions for strategic planning
/// </summary>
public class Detection : MonoBehaviour, ISkill
{
    [Header("Assigned Via Execution time -- Dont touch")]
    [SerializeField] private SkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;

    [Header("Detection Settings")]
    [Tooltip("Duration in seconds that enemies remain visible on minimap")]
    public float detectionDuration = 5f;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject vfx;
    private AudioSource audioSource;

    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config;
        this.user = user;
        audioSource = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        if (!CanActivate())
        {
            Debug.Log("[Detection] Skill on cooldown!");
            return;
        }

        lastUsedTime = Time.time;

        // Use configured duration or fallback to serialized field
        float duration = config.skillLifetime > 0 ? config.skillLifetime : detectionDuration;

        // Reveal all enemies on minimap
        if (MinimapManager.Instance != null)
        {
            Debug.Log($"[Detection] ===== DETECTION SKILL ACTIVATED ===== Revealing enemies for {duration} seconds");
            MinimapManager.Instance.RevealEnemiesTemporarily(duration);
        }
        else
        {
            Debug.LogError("[Detection] MinimapManager instance not found! Make sure MinimapManager is in the scene.");
        }

        // Spawn visual effect
        if (config.vfxPrefab != null)
        {
            vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity);
            Destroy(vfx, duration);
        }

        // Play sound effect
        if (audioSource != null)
        {
            audioSource.Play();
        }
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
        // Clean up VFX if skill holder is destroyed
        if (vfx != null)
        {
            Destroy(vfx);
        }
    }
}
