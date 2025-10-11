using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInitiator : MonoBehaviour
{
    public static GameInitiator Instance { get; private set; }
    [Header("Core Managers (Prefab Fallbacks)")]
    [SerializeField] private GameStateManager gameStateManagerPrefab;
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private InputService inputServicePrefab;
    [SerializeField] private SoundManager soundManagerPrefab;
    [SerializeField] private UIManager uIManager;

    [Header("Development Flags")]
    public bool isDevelopment = true;
    public bool generateTestData = false;

    [Header("Gameplay Settings (Dev Only)")]
    [SerializeField] private LevelData currentLevel;

    [Header("Flags")]
    public bool isGenerated = false; // Flagged as public for independent monobehaviour scripts
    public bool isFinished = false;
    ////////////////////////////////////////////////////
    // Runtime references
    public GameStateManager GameStateManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public InputService InputService { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public UIManager UIManager { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private async UniTaskVoid Start()
    {
        Debug.Log("[GameInitiator] Starting...");

        await BindObjects();
        await Initialize();
        await PrepareCoreSystems();

        await PrepareGame(new SaveData());


        isGenerated = true;
    }
    private async UniTask BindObjects()
    {
        Debug.Log("[GameInitiator] Binding managers...");

        GameStateManager = EnsureExists(GameStateManager, gameStateManagerPrefab);
        GameManager = EnsureExists(GameManager, gameManagerPrefab);
        InputService = EnsureExists(InputService, inputServicePrefab);
        SoundManager = EnsureExists(SoundManager, soundManagerPrefab);
        UIManager = EnsureExists(UIManager, uIManager);

        await UniTask.CompletedTask;
    }

    private T EnsureExists<T>(T existing, T prefab) where T : MonoBehaviour
    {
        if (existing != null) return existing;

        var found = FindAnyObjectByType<T>();
        if (found != null) return found;

        if (prefab == null)
            throw new Exception($"{typeof(T).Name} is missing in GameInitiator. Please assign a prefab.");

        return Instantiate(prefab);
    }
    private async UniTask Initialize()
    {
        Debug.Log("[GameInitiator] Initializing managers...");

        await GameManager.Initialize();
        await GameStateManager.Intialize();
        await InputService.Initialize();
        await SoundManager.Initialize();
    }

    public async UniTask PrepareCoreSystems()
    {
        GameManager.PlayerDataManager.Initialize();

        if (isDevelopment)
        {
            Debug.Log("[GameInitiator] Dev environment detected.");
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var devState = GameStateUtils.GetStateFromSceneName(currentScene);
            await GameStateManager.SetState(devState);
        }

        Debug.Log("[GameInitiator] Core systems prepared.");
    }
    public async UniTask PrepareGame(SaveData saveData)
    {
        await GameManager.PlayerManager.Initialize(saveData.playerData);
        await GameManager.CharacterManager.Initialize(saveData.ownedCharacters);
        await GameManager.TeamManager.Initialize(saveData.teams);
        await GameManager.AntiVirusManager.Initialize();
        await GameManager.LevelManager.Initialize();
        await GameManager.StatusEffectManager.Initialize();

        await UniTask.Yield();

        isFinished = true;
        Debug.Log("[GameInitiator] Game preparation complete.");
    }

    public async void SwitchStates(GameState state)
    {
        await GameStateManager.SetState(state);
    }
}
