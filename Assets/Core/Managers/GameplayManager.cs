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
    public string previousTeamID;
    public Dictionary<string, bool> deadTeams = new Dictionary<string, bool>();

    // Events
    public event Action onUpdateDeadTeam;

    public enum GameplayState
    {
        None,
        Start,
        Playing,
        Revive,
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
        Debug.Log("[GameplayManager] Waiting for GameInitiator...");
        await UniTask.WaitUntil(() =>
            GameInitiator.Instance != null && GameInitiator.Instance.isFinished);

        Debug.Log("[GameplayManager] Initialization Passed");
        Initialize();

        await SetState(GameplayState.Start);
    }
    public void Initialize()
    {
        if (_isInitialized) return;

        // Core setup
        inputService = GameInitiator.Instance.InputService;
        gameManager = GameInitiator.Instance.GameManager;

        gameplayUI.Initialize(gameManager.PlayerManager.playerService);
        enemyManager.Initialize();
        waveManager.Initialize();
        parallaxBackground.Initialize();

        Debug.Log("[GameplayManager] Initialized");
        _isInitialized = true;
    }
    public async UniTask SetState(GameplayState newState)
    {
        if (currentState == newState)
        {
            Debug.Log($"[GameplayManager] State '{newState}' ignored (already active).");
            return;
        }

        // Exit old state
        switch (currentState)
        {
            case GameplayState.Start:
                // Cleanup Start state if needed
                break;

            case GameplayState.Playing:
                // Pause gameplay, stop timers, etc.
                break;

            case GameplayState.End:
                // Cleanup end screen
                break;
        }

        currentState = newState;

        // Enter new state
        switch (currentState)
        {
            case GameplayState.Start:
                Debug.Log($"[GameplayManager] isGameActive before Start: {isGameActive}");
                if (!isGameActive) HandleStart();
                if (gameplayUI != null)
                    await gameplayUI.StartStateUIAnimation();
                isGameActive = true;
                break;

            case GameplayState.Playing:
                HandleTeamChange();
                if (gameplayUI != null)
                    await gameplayUI.PlayingStateUIAnimation();
                StartWave();
                break;

            case GameplayState.Revive:
                enemyManager.ResetTargets();
                waveManager.PauseWave(true);
                break;

            case GameplayState.End:
                enemyManager.KillAllEnemies(true);
                gameplayUI?.HandleEndGamePanel();
                break;
        }
    }

    public void HandleStart()
    {
        LevelData currentLevel = GameManager.Instance.LevelManager.activeLevel;

        // Background / Environment
        parallaxBackground.SetupParallaxLayerMaterial(currentLevel.background);

        // Waves
        waveManager.SetWaveConfig(currentLevel.waveSet.waves);

        // Audio
        SoundManager.PlaySound(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);

        // UI Setup
        gameplayUI.StartStateSetup();

        // Teams
        List<TeamService> teams = GameManager.Instance.TeamManager.GetActiveTeam();
        foreach (TeamService team in teams)
        {
            deadTeams[team.GetData().teamID] = false;
        }

        // Register initial team
        activeTeamID = teams[0].GetData().teamID;
        HandleTeamChange();

        Debug.Log("[GameplayManager] Gameplay Active and Start State Initialized");
    }
    public void HandleTeamChange()
    {
        // Only refresh if the active team actually changed
        if (previousTeamID == activeTeamID)
            return;

        previousTeamID = activeTeamID;

        List<TeamService> teams = GameManager.Instance.TeamManager.GetActiveTeam();
        List<CharacterData> members = teams[0].GetMembers();

        // Build battle states
        List<CharacterBattleState> battleStates = members
            .Select(m => new CharacterBattleState(new CharacterService(m)))
            .ToList();

        // Initialize followers + player control
        List<GameObject> followerObjects = followerManager.Initialize(battleStates);
        playerGameplayManager.Initialize(followerObjects, inputService);

        // Setup UI
        gameplayUI.SetupCharacterUI(battleStates);

        // Default focus to first follower
        followerManager.SwitchTo(0);

    }
    public async void HandleEndGame()
    {
        // TODO: Implement end game logic (reward distribution, summary, etc.)
        Debug.Log("[GameplayManager] End game sequence triggered.");
        await SetState(GameplayState.End);
    }
    public void StartWave()
    {
        if (waveManager.currentWave == null)
            waveManager.StartNextWave();
        else
            waveManager.PauseWave(false);
    }
    public void SetDeadTeam(string teamID, bool isDead)
    {
        if (!deadTeams.ContainsKey(teamID))
            return;

        deadTeams[teamID] = isDead;
        onUpdateDeadTeam?.Invoke();
    }
}
