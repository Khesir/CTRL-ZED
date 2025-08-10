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
    [Header("Bindable Objects")]
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputService inputService;
    [Header("Environment Setup")]
    [SerializeField] private GameState initialState = GameState.Initial;
    [Header("Flags")]
    public bool isDevelopment = true;
    public bool isGenerated = false; // Flagged as public for independent monobehaviour scripts
    public bool isFinished = false;
    public bool generateTestData = false;
    [Header("Gameplay Settings - Dev Settings")]
    [SerializeField] private LevelData currentLevel;
    ////////////////////////////////////////////////////
    private GameStateManager _gameStateManager;
    private GameManager _gameManager;
    private InputService _inputService;
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
    private async void Start()
    {
        Debug.Log("[GameInitiator] Generating Bindable Objects");
        await BindObjects();
        Debug.Log("[GameInitiator] Initializing Objects");
        await Initialize();
        Debug.Log("[GameInitiator] Preparing Core Systems");
        PrepareCoreSystems();
        // Debug.Log("[GameInitiator] Preparing Game");
        // await PrepareGame();
        // Start of the game
        isGenerated = true;
    }
    private async UniTask BindObjects()
    {
        // This handles shared managers that has yet to exists or already on the scene to ensure loading
        if (_gameStateManager == null)
        {
            _gameStateManager = FindObjectOfType<GameStateManager>();
            if (_gameStateManager == null)
            {
                if (gameStateManager == null)
                {
                    throw new System.Exception("GameStateManager reference is missing in GameInitiator. Please assign it in the inspector.");
                }
                _gameStateManager = Instantiate(gameStateManager);
            }
        }

        if (_gameManager == null)
        {
            _gameManager = FindAnyObjectByType<GameManager>();
            if (_gameManager == null)
            {
                if (gameManager == null)
                {
                    throw new System.Exception("GameManager reference is missing in GameInitiator. Please assign it in the inspector.");
                }
                _gameManager = Instantiate(gameManager);
            }
        }
        if (_inputService == null)
        {
            _inputService = FindObjectOfType<InputService>();
            if (_inputService == null)
            {
                if (inputService == null)
                {
                    throw new System.Exception("InputService reference is missing in GameInitiator. Please assign it in the inspector.");
                }
                _inputService = Instantiate(inputService);
            }
        }
        await UniTask.CompletedTask;
    }
    private async UniTask Initialize()
    {
        // Initialize global Systems
        await _gameManager.Initialize();
        await _gameStateManager.Intialize();
        await _inputService.Initialize();
    }
    public async UniTask PrepareCoreSystems()
    {
        GameManager.Instance.PlayerDataManager.Initialize();

        if (generateTestData) GenerateTestData();

        if (isDevelopment)
        {
            Debug.Log("[GameInitiator] Development Environment Detected");
            // Don't load initial scene, just sync current scene to GameStateManager
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameState devState = GameStateUtils.GetStateFromSceneName(currentScene);
            await _gameStateManager.SetState(devState);
        }


    }
    public async UniTask PrepareGame(SaveData saveData)
    {
        await GameManager.Instance.PlayerManager.Initialize(saveData.playerData);
        await GameManager.Instance.CharacterManager.Initialize(saveData.ownedCharacters);
        await GameManager.Instance.TeamManager.Initialize(saveData.teams);
        await GameManager.Instance.AntiVirusManager.Initialize();
        await GameManager.Instance.LevelManager.Initialize();

        Debug.Log("[GameInitiator] Game preparation (player data) complete.");
        if (generateTestData) GenerateTestData();

        Debug.Log("[GameInitiator] Game preparation (player State) complete.");
        isFinished = true;
        await UniTask.CompletedTask;
    }
    private void GenerateTestData()
    {
        Debug.Log("[GameInitiator] Generating Test usable Data");
        var teamManager = GameManager.Instance.TeamManager;
        var teamID = teamManager.CreateTeam();
        var character = GameManager.Instance.CharacterManager.GetCharacters();

        var character1 = CharacterFactory.CreateTestCharacter();
        var character2 = CharacterFactory.CreateTestCharacter();
        var character3 = CharacterFactory.CreateTestCharacter();
        var character4 = CharacterFactory.CreateTestCharacter();

        // Just to ensure we owned the character
        character.Add(character1);
        character.Add(character2);
        character.Add(character3);
        character.Add(character4);
        // Assigned owned character to slot
        teamManager.AssignedCharacterToSlot(teamID, 0, character1);
        teamManager.AssignedCharacterToSlot(teamID, 1, character2);
        teamManager.AssignedCharacterToSlot(teamID, 2, character3);
        teamManager.AssignedCharacterToSlot(teamID, 3, character4);
        teamManager.SetActiveTeam(teamID);
        GameManager.Instance.LevelManager.activeLevel = currentLevel;

        Debug.Log("[GameInitiator] Generating Data Successfull");
    }
    public GameManager GetGameManager() => _gameManager;
    public GameStateManager GetGameStateManager() => _gameStateManager;
    public InputService GetInputService() => _inputService;
    public async void SwitchStates(GameState state)
    {
        await _gameStateManager.SetState(state);
    }
}
