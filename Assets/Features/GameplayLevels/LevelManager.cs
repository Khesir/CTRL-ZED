using System;
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
        // Check active team and check if active team match the required level;
        var teamManger = ServiceLocator.Get<ITeamManager>();
        bool found = false;

        foreach (var team in teamManger.GetActiveTeam())
        {
            Debug.Log($"Checking team: {team.GetData().teamID}");

            foreach (var member in team.GetMembers())
            {
                Debug.Log($"Checking member: {member.baseData.className} - Level {member.currentLevel}");

                foreach (var levelData in level.characterRequirements)
                {
                    Debug.Log($"Comparing against requirement: {levelData.character.className} - Required Level {levelData.levelRequirement}");

                    if (!string.Equals(member.baseData.className, levelData.character.className, StringComparison.Ordinal))
                    {
                        Debug.Log($"Class mismatch: {member.baseData.className} != {levelData.character.className}");
                        continue;
                    }

                    if (member.currentLevel != levelData.levelRequirement)
                    {
                        Debug.Log($"Level mismatch: {member.currentLevel} != {levelData.levelRequirement}");
                        continue;
                    }

                    Debug.Log($"Requirement matched for member: {member.baseData.className} - Level {member.currentLevel}");
                    found = true;
                    break; // Requirement matched, break inner loop
                }

                if (found)
                {
                    Debug.Log("Found matching member, stopping further checks.");
                    break; // Exit member loop
                }
            }

            if (found)
            {
                Debug.Log("Found matching team, stopping further team checks.");
                break; // Exit team loop
            }
        }

        if (!found)
        {
            Debug.Log("No matching member found in any active team. Level cannot be unlocked.");
            return false;
        }

        // Check if previous level is complete (linear progression)
        LevelData previousLevel = allLevels[levelIndex - 1];
        return IsLevelComplete(previousLevel.levelID);
    }
}
