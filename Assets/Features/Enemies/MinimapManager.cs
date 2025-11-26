using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Manages minimap visibility for enemies
    /// Controls the "MInimap Icon" child objects on Layer 14
    /// </summary>
    public class MinimapManager : MonoBehaviour
    {
        private static MinimapManager instance;
        public static MinimapManager Instance => instance;

        [Header("Minimap Settings")]
        [SerializeField] private bool showEnemiesByDefault = false;
        [SerializeField] private string minimapIconName = "MInimap Icon";
        [SerializeField] private int minimapLayer = 14;
        [Tooltip("If true, disables the GameObject; if false, only disables the SpriteRenderer")]
        [SerializeField] private bool disableGameObject = true;

        // Track all registered enemies
        private readonly List<GameObject> registeredEnemies = new List<GameObject>();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            Debug.Log("[MinimapManager] Initialized - Enemies hidden by default: " + !showEnemiesByDefault);
        }

        private void Start()
        {
            // Hide all enemies that might have spawned before this manager initialized
            if (!showEnemiesByDefault)
            {
                HideAllExistingEnemies();
            }
        }

        /// <summary>
        /// Find and hide all enemies already in the scene (called at start)
        /// </summary>
        private void HideAllExistingEnemies()
        {
            // Find all GameObjects with EnemyService component
            EnemyService[] existingEnemies = FindObjectsOfType<EnemyService>();
            Debug.Log($"[MinimapManager] Found {existingEnemies.Length} existing enemies to hide");

            foreach (var enemy in existingEnemies)
            {
                if (enemy != null)
                {
                    RegisterEnemy(enemy.gameObject);
                }
            }
        }

        /// <summary>
        /// Called when a new enemy is spawned to set initial minimap visibility
        /// </summary>
        public void RegisterEnemy(GameObject enemyObject)
        {
            if (enemyObject != null && !registeredEnemies.Contains(enemyObject))
            {
                registeredEnemies.Add(enemyObject);
                SetEnemyMinimapVisibility(enemyObject, showEnemiesByDefault);
            }
        }

        /// <summary>
        /// Called when an enemy is destroyed
        /// </summary>
        public void UnregisterEnemy(GameObject enemyObject)
        {
            registeredEnemies.Remove(enemyObject);
        }

        /// <summary>
        /// Show all enemies on the minimap
        /// </summary>
        public void ShowAllEnemies()
        {
            // Clean up null references
            registeredEnemies.RemoveAll(e => e == null);

            Debug.Log($"[MinimapManager] SHOWING {registeredEnemies.Count} enemies on minimap");

            foreach (var enemy in registeredEnemies)
            {
                if (enemy != null)
                {
                    SetEnemyMinimapVisibility(enemy, true);
                }
            }
        }

        /// <summary>
        /// Hide all enemies from the minimap
        /// </summary>
        public void HideAllEnemies()
        {
            // Clean up null references
            registeredEnemies.RemoveAll(e => e == null);

            Debug.Log($"[MinimapManager] HIDING {registeredEnemies.Count} enemies on minimap");

            foreach (var enemy in registeredEnemies)
            {
                if (enemy != null)
                {
                    SetEnemyMinimapVisibility(enemy, false);
                }
            }
        }

        /// <summary>
        /// Toggle minimap visibility for a specific enemy
        /// </summary>
        private void SetEnemyMinimapVisibility(GameObject enemyObject, bool visible)
        {
            if (enemyObject == null) return;

            // Find the minimap icon child object
            Transform minimapIcon = enemyObject.transform.Find(minimapIconName);
            if (minimapIcon != null)
            {
                if (disableGameObject)
                {
                    // Disable/enable the entire GameObject
                    minimapIcon.gameObject.SetActive(visible);
                    Debug.Log($"[MinimapManager] Set minimap icon GameObject for {enemyObject.name} to {(visible ? "ACTIVE" : "INACTIVE")}");
                }
                else
                {
                    // Enable/disable the SpriteRenderer only
                    SpriteRenderer spriteRenderer = minimapIcon.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.enabled = visible;
                        Debug.Log($"[MinimapManager] Set minimap icon SpriteRenderer for {enemyObject.name} to {(visible ? "ENABLED" : "DISABLED")}");
                    }
                    else
                    {
                        Debug.LogWarning($"[MinimapManager] No SpriteRenderer found on minimap icon for {enemyObject.name}");
                    }
                }
            }
            else
            {
                // Try to find with different name variations in case of typo
                string[] possibleNames = { "Minimap Icon", "minimap icon", "MinimapIcon", "Minimap_Icon" };
                foreach (string altName in possibleNames)
                {
                    minimapIcon = enemyObject.transform.Find(altName);
                    if (minimapIcon != null)
                    {
                        Debug.LogWarning($"[MinimapManager] Found minimap icon with alternate name '{altName}' instead of '{minimapIconName}'");
                        minimapIconName = altName; // Update for future use
                        SetEnemyMinimapVisibility(enemyObject, visible); // Retry with correct name
                        return;
                    }
                }

                Debug.LogWarning($"[MinimapManager] No child '{minimapIconName}' found on {enemyObject.name}. Children are: {string.Join(", ", GetChildNames(enemyObject.transform))}");
            }
        }

        private string[] GetChildNames(Transform parent)
        {
            string[] names = new string[parent.childCount];
            for (int i = 0; i < parent.childCount; i++)
            {
                names[i] = parent.GetChild(i).name;
            }
            return names;
        }

        /// <summary>
        /// Show enemies for a duration, then hide them again
        /// </summary>
        public void RevealEnemiesTemporarily(float duration)
        {
            ShowAllEnemies();
            Invoke(nameof(HideAllEnemies), duration);
        }

        /// <summary>
        /// Cancel any pending hide operations
        /// </summary>
        public void CancelScheduledHide()
        {
            CancelInvoke(nameof(HideAllEnemies));
        }
    }
}
