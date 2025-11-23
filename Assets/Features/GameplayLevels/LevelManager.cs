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
}
