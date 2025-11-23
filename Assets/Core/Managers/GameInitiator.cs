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


    [Header("Tutorial")]
    [SerializeField] private LevelData tutorialLevel;
    [SerializeField] private bool skipTutorial = false;
    public bool isInTutorial = false;
    ////////////////////////////////////////////////////
    // Runtime references
    public GameStateManager GameStateManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public InputService InputService { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public CoreEventBus CoreEventBus { get; private set; }

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
        RegisterServices();
        await Initialize();
        await PrepareCoreSystems();

        isFinished = true;
    }
    private async UniTask BindObjects()
    {
        Debug.Log("[GameInitiator] Binding managers...");

        GameStateManager = EnsureExists(GameStateManager, gameStateManagerPrefab);
        GameManager = EnsureExists(GameManager, gameManagerPrefab);
        InputService = EnsureExists(InputService, inputServicePrefab);
        SoundManager = EnsureExists(SoundManager, soundManagerPrefab);
        UIManager = EnsureExists(UIManager, uIManager);

        // Create CoreEventBus if it doesn't exist
        CoreEventBus = FindAnyObjectByType<CoreEventBus>();
        if (CoreEventBus == null)
        {
            var eventBusGO = new GameObject("CoreEventBus");
            CoreEventBus = eventBusGO.AddComponent<CoreEventBus>();
            eventBusGO.transform.SetParent(transform);
        }

        isGenerated = true;
        await UniTask.CompletedTask;
    }

    private T EnsureExists<T>(T existing, T prefab) where T : MonoBehaviour
    {
        if (existing != null) return existing;

        var found = FindAnyObjectByType<T>();
        if (found != null)
        {
            // Make persistent by parenting to GameInitiator
            found.transform.SetParent(transform);
            return found;
        }

        if (prefab == null)
            throw new Exception($"{typeof(T).Name} is missing in GameInitiator. Please assign a prefab.");

        var instance = Instantiate(prefab);
        instance.transform.SetParent(transform); // Make child of GameInitiator for persistence
        return instance;
    }

    private void RegisterServices()
    {
        Debug.Log("[GameInitiator] Registering services...");

        // Initialize DI Container
        var container = new DIContainer();
        GameServices.Initialize(container);

        // Configure core services via composition root
        CoreCompositionRoot.Configure(
            container,
            SoundManager,
            InputService,
            GameManager,
            GameStateManager,
            this // Register self for tutorial flags
        );

    }

    private async UniTask Initialize()
    {
        Debug.Log("[GameInitiator] Initializing managers...");

        await GameManager.Initialize();
        await GameStateManager.Intialize();
        await InputService.Initialize();
        SoundManager.Initialize();
    }

    public async UniTask PrepareCoreSystems()
    {
        ServiceLocator.Get<IPlayerDataManager>().Initialize();

        if (isDevelopment)
        {
            Debug.Log("[GameInitiator] Dev environment detected.");
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            // In dev mode, initialize services with test data if not in StartMenu
            if (currentScene != "StartMenu")
            {
                Debug.Log("[GameInitiator] Dev mode: Initializing services with default data...");
                var testData = new SaveData();
                await PrepareGameDev(testData, currentScene);
                return;
            }

            var devState = GameStateUtils.GetStateFromSceneName(currentScene);
            await GameStateManager.SetState(devState);
        }

        Debug.Log("[GameInitiator] Core systems prepared.");
    }

    private async UniTask PrepareGameDev(SaveData saveData, string currentScene)
    {
        await ServiceLocator.Get<IPlayerManager>().Initialize(saveData.playerData);
        await ServiceLocator.Get<ICharacterManager>().Initialize(saveData.ownedCharacters);
        await ServiceLocator.Get<ITeamManager>().Initialize(saveData.teams);
        await ServiceLocator.Get<IAntiVirusManager>().Initialize();
        await ServiceLocator.Get<ILevelManager>().Initialize();
        await ServiceLocator.Get<IStatusEffectManager>().Initialize();

        await UniTask.Yield();

        // For Gameplay scene in dev mode, use currentLevel if set, otherwise tutorialLevel
        if (currentScene == "Gameplay")
        {
            var levelToUse = currentLevel != null ? currentLevel : tutorialLevel;
            var levelManager = ServiceLocator.Get<ILevelManager>();

            Debug.Log($"[GameInitiator] Dev mode - currentLevel field: '{currentLevel?.levelName ?? "NULL"}'");
            Debug.Log($"[GameInitiator] Dev mode - levelToUse: '{levelToUse?.levelName ?? "NULL"}'");
            Debug.Log($"[GameInitiator] Dev mode - LevelManager.activeLevel BEFORE: '{levelManager.activeLevel?.levelName ?? "NULL"}'");

            levelManager.activeLevel = levelToUse;
            isInTutorial = (levelToUse == tutorialLevel);

            Debug.Log($"[GameInitiator] Dev mode - LevelManager.activeLevel AFTER: '{levelManager.activeLevel?.levelName ?? "NULL"}'");
            SwitchStates(GameState.Gameplay);
        }
        else
        {
            // For other scenes, just switch to the appropriate state
            var devState = GameStateUtils.GetStateFromSceneName(currentScene);
            SwitchStates(devState);
        }

        Debug.Log("[GameInitiator] Dev game preparation complete.");
    }
    public async UniTask PrepareGame(SaveData saveData)
    {
        await ServiceLocator.Get<IPlayerManager>().Initialize(saveData.playerData);
        await ServiceLocator.Get<ICharacterManager>().Initialize(saveData.ownedCharacters);
        await ServiceLocator.Get<ITeamManager>().Initialize(saveData.teams);
        await ServiceLocator.Get<IAntiVirusManager>().Initialize();
        await ServiceLocator.Get<ILevelManager>().Initialize();
        await ServiceLocator.Get<IStatusEffectManager>().Initialize();

        await UniTask.Yield();

        // Determine next state based on tutorial completion
        bool goToTutorial = !skipTutorial &&
            !ServiceLocator.Get<IPlayerManager>().playerService.GetPlayerData().completedTutorial;

        if (goToTutorial)
        {
            ServiceLocator.Get<ILevelManager>().activeLevel = tutorialLevel;
            isInTutorial = true;
            Debug.Log("[GameInitiator] Tutorial not completed. Going to Gameplay (Tutorial).");
            SwitchStates(GameState.Gameplay);
        }
        else
        {
            Debug.Log("[GameInitiator] Going to MainMenu.");
            SwitchStates(GameState.MainMenu);
        }

        Debug.Log("[GameInitiator] Game preparation complete.");
    }
    public bool introViewed = false;
    public void IntroViewed() => introViewed = true;
    public void CompleteTutorial(bool status = true)
    {
        skipTutorial = status;
        isInTutorial = !status;
    }

    public async void SwitchStates(GameState state)
    {
        await GameStateManager.SetState(state);
    }
}
