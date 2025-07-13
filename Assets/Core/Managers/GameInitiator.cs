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
    public GameStateManager gameStateManager;
    public GameManager gameManager;

    [Header("Environment Setup")]
    [SerializeField] private GameState initialState = GameState.Initial;
    [SerializeField] private bool isDevelopment = true;
    [Header("Flags")]
    [SerializeField] public bool isGenerated = false;
    private GameStateManager _gameStateManager;
    private GameManager _gameManager;

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
        Debug.Log("[GameInitiator] Preparing Game");
        await PrepareGame();
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
        await UniTask.CompletedTask;
    }
    private async UniTask Initialize()
    {
        // Initialize global Systems
        await gameManager.Initialize();
        await gameStateManager.Intialize();
    }
    private async UniTask PrepareGame()
    {
        var saveData = await GameManager.Instance.PlayerDataManager.Initialize();

        await GameManager.Instance.PlayerManager.Initialize(saveData.playerData);
        await GameManager.Instance.TeamManager.Initialize(saveData.teams);
        await GameManager.Instance.CharacterManager.Initialize(saveData.ownedCharacters);
        await GameManager.Instance.AntiVirusManager.Initialize(saveData.antiVirusLevel);

        Debug.Log("[GameInitiator] Game preparation (player data) complete.");
        // Game Preparation
        if (isDevelopment)
        {
            // Don't load initial scene, just sync current scene to GameStateManager
            var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameState devState = GameStateUtils.GetStateFromSceneName(currentScene);
            gameStateManager.SetState(devState);
        }
        else
        {
            gameStateManager.SetState(initialState);
        }

        Debug.Log("[GameInitiator] Game preparation (player State) complete.");
        await UniTask.CompletedTask;
    }
}
