using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager Instance { get; private set; }
    public bool isGameActive = false;
    public bool _isInitialized = false;
    [Header("Core References")]
    public GameManager gameManager;
    public GameplayUIController gameplayUI;

    [SerializeField] private IInputService inputService;

    [Header("Gameplay Systems ")]
    public ParallaxBackground parallaxBackground;
    public FollowerSpawn spawn;
    public SquadLevelManager squadLevelManager; // Not used
    public DamageNumberController damageNumberController;
    public EnemySpawner spawner;
    public EnemyManager enemyManager;
    public WaveManager waveManager;
    public LootManager lootManager;

    [Header("Player-Dependent Systems")]
    public FollowerManager followerManager;
    public PlayerGameplayManager playerGameplayManager;


    // Gameplay Main Flags & Parameters
    [Header("Gameplay Flags & Parameters")]
    public string activeTeamID;
    public Dictionary<string, bool> deadTeams = new Dictionary<string, bool>();

    // Events
    public event Action onUpdateDeadTeam;

    public enum GameplayState
    {
        None,
        Start,
        Playing,
        End
    }

    private GameplayState currentState = GameplayState.None;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private async void Start()
    {
        Debug.Log("[GameplayManager] Waiting");

        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        Debug.Log("[GameplayManager] Passed");
        Initialize();
        await SetState(GameplayState.Start);
    }
    public void Initialize()
    {
        if (_isInitialized) return;

        // Check all core Managers;
        inputService = GameInitiator.Instance.InputService;
        gameManager = GameInitiator.Instance.GameManager;

        // Initialize data level
        gameplayUI.Initialize(gameManager.PlayerManager.playerService);
        enemyManager.Initialize();
        waveManager.Initialize();

        // SetEnvironment
        parallaxBackground.Initialize();
        Debug.Log("[GameplayManager] Gameplay Manager Initialized");
        _isInitialized = true;
    }
    public async UniTask SetState(GameplayState newState)
    {
        if (currentState == newState) return;

        // Exit previous state
        // Handles cleaning
        switch (currentState)
        {
            case GameplayState.Start:
                // Clean up Start state if needed
                // No usual Setup unless we doig endless mode state transition from end -> start
                break;
            case GameplayState.Playing:
                // Pause gameplay, stop timers, etc.
                break;
            case GameplayState.End:
                // Cleanup end screen
                // Pause Gameplay etc.. 
                // Remove active ui that is in playing state
                break;
        }

        currentState = newState;

        // Enter new state
        switch (currentState)
        {
            case GameplayState.Start:
                HandleStart(); // Gameplay logics
                await gameplayUI.StartStateUIAnimation();
                isGameActive = true; // Control Flag
                break;
            case GameplayState.Playing:
                await gameplayUI.PlayingStateUIAnimation();
                waveManager.StartNextWave();
                break;
            case GameplayState.End:
                HandleEndGame();
                break;
        }
    }

    public void HandleStart()
    {
        // LevelData
        LevelData currentLevel = GameManager.Instance.LevelManager.activeLevel;

        // Environments        
        parallaxBackground.SetupParallaxLayerMaterial(currentLevel.background);

        // Waves
        waveManager.SetWaveConfig(currentLevel.waveSet.waves);

        // Sound
        SoundManager.PlaySound(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);

        // UI
        gameplayUI.StartStateSetup();

        // Get list of active team and set team 0 as first deployed team
        List<TeamService> teams = GameManager.Instance.TeamManager.GetActiveTeam();
        List<CharacterData> members = teams[0].GetMembers();

        // Add to Dictionary for fast lookup
        foreach (TeamService team in teams)
        {
            deadTeams.Add(team.GetData().teamID, false);
        }
        // Register Active tteam
        activeTeamID = teams[0].GetData().teamID;

        // Registering battlestates of all Characters, player logics
        List<CharacterBattleState> battleStates = members.Select(m => new CharacterBattleState(new CharacterService(m))).ToList();

        // Handling characters
        List<GameObject> GO = followerManager.Initialize(battleStates);
        playerGameplayManager.Initialize(GO, inputService);
        gameplayUI.SetupCharacterUI(battleStates);

        followerManager.SwitchTo(0);

        Debug.Log("[GameplayManager] Gameplay Manager is now Active");
    }
    private void HandleEndGame()
    {
        // Show end screen, play sounds, etc.
        // Destroy gameplay objects if needed
        Destroy(gameObject);
    }

    public void SetDeadTeam(string teamID, bool isDead)
    {
        if (deadTeams.ContainsKey(teamID))
        {
            // Register Dead Teams
            deadTeams[activeTeamID] = isDead;
            onUpdateDeadTeam?.Invoke();
        }
    }
}
