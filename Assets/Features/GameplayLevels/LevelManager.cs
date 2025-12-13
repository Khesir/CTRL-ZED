using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{

    public static LevelManager Instance { get; private set; }

    // Proper interface implementation
    LevelData ILevelManager.activeLevel
    {
        get => activeLevel;
        set => activeLevel = value;
    }

    List<LevelData> ILevelManager.allLevels => allLevels;

    [Header("Assigned via Inspector or Resources.Load")]
    public LevelData activeLevel;
    public List<LevelData> allLevels = new();

    private Dictionary<string, LevelData> levelLookup;
    private HashSet<string> clearedLevelIDs = new HashSet<string>();
    public GameObject loaderCanva;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }
    public async UniTask Initialize()
    {

        if (allLevels.Count == 0)
            allLevels.AddRange(Resources.LoadAll<LevelData>("Levels"));

        levelLookup = allLevels.ToDictionary(level => level.levelID);
        Debug.Log("[LevelManager] Level Manager Initialized");
        await UniTask.CompletedTask;
    }
    public LevelData GetLevelByID(string id)
    {
        return levelLookup.TryGetValue(id, out var data) ? data : null;
    }
    public List<LevelData> GetAllLevels() => allLevels;
    public LevelData GetActiveLevel() => activeLevel;
    // This is depends on GameStateManager
    public async UniTask LoadScene(GameState gameState)
    {
        Debug.Log("[LevelManager] Preparing for the mission");
        await GameInitiator.Instance.GameStateManager.SetState(gameState);
        Debug.Log("[LevelManager] Preparation Complete!");
    }

    // Level unlock and completion methods
    public void LoadClearedLevels(List<string> clearedLevels)
    {
        clearedLevelIDs.Clear();
        if (clearedLevels != null)
        {
            foreach (var levelID in clearedLevels)
            {
                clearedLevelIDs.Add(levelID);

                // Update LevelData.isCleared based on player progression
                if (levelLookup.TryGetValue(levelID, out var levelData))
                {
                    levelData.isCleared = true;
                }
            }
        }
        Debug.Log($"[LevelManager] Loaded {clearedLevelIDs.Count} cleared levels");
    }

    public bool IsLevelComplete(string levelID)
    {
        return clearedLevelIDs.Contains(levelID);
    }

    public void MarkLevelComplete(string levelID)
    {
        if (!clearedLevelIDs.Contains(levelID))
        {
            clearedLevelIDs.Add(levelID);

            // Update LevelData.isCleared based on player progression
            if (levelLookup.TryGetValue(levelID, out var levelData))
            {
                levelData.isCleared = true;
            }

            Debug.Log($"[LevelManager] Level {levelID} marked as complete");
        }
    }

    public bool IsLevelUnlocked(LevelData level)
    {
        if (level == null) return false;

        // First level is always unlocked
        if (allLevels.Count > 0 && allLevels[0] == level)
        {
            return true;
        }

        // Find the index of this level
        int levelIndex = allLevels.IndexOf(level);
        if (levelIndex <= 0) return false; // Not found or is first level

        // Lock if os level is still 
        int currentLevel = ServiceLocator.Get<IPlayerManager>().playerService.GetLevel();
        if (level.OsLevelRequirement > currentLevel) return false;

        // Check if previous level is complete (linear progression)
        LevelData previousLevel = allLevels[levelIndex - 1];
        return IsLevelComplete(previousLevel.levelID);
    }
}
