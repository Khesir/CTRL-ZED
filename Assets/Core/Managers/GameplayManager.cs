using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum GameplayState { None, Start, Playing, Revive, End }
public enum GameplayEndGameState { DeathOnTimer, LevelComplete }

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    [Header("Core References")]
    [SerializeField] private GameplayUIController gameplayUI;
    [SerializeField] private ParallaxBackground parallaxBackground;
    [SerializeField] private DamageNumberService damageNumberService;

    [Header("Gameplay Systems")]
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private LootManager lootManager;

    [Header("Player Systems")]
    [SerializeField] private FollowerManager followerManager;
    [SerializeField] private PlayerGameplayManager playerGameplayManager;

    [Header("Tutorial")]
    [SerializeField] private LevelData tutorialLevel;

    // Public accessors for systems that still need direct access
    public GameplayUIController GameplayUI => gameplayUI;
    public IFollowerManager FollowerManager => followerManager;
    public IEnemyManager EnemyManager => enemyManager;
    public IWaveManager WaveManager => waveManager;
    public string ActiveTeamID
    {
        get => activeTeamID;
        set
        {
            activeTeamID = value;
            HandleTeamChange();
        }
    }

    // State
    public bool IsGameActive { get; private set; }
    public GameplayEndGameState EndGameState { get; set; }
    public GameplayState CurrentState { get; private set; } = GameplayState.None;

    // Team tracking
    private string activeTeamID;
    private string previousTeamID;
    private readonly Dictionary<string, bool> deadTeams = new();

    public event Action OnDeadTeamUpdated;

    // Cached references
    private GameManager gameManager;
    private IInputService inputService;
    private ISoundService soundService;
    private IPlayerManager playerManager;
    private bool isInitialized;

    // State Machine
    private StateMachine stateMachine;
    private GameplayStartState startState;
    private GameplayPlayingState playingState;
    private GameplayReviveState reviveState;
    private GameplayEndState endState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);

        Initialize();
        await SetState(GameplayState.Start);
    }

    private void Initialize()
    {
        if (isInitialized) return;

        // Create SceneEventBus for this scene if it doesn't exist
        if (FindAnyObjectByType<SceneEventBus>() == null)
        {
            var sceneEventBusGO = new GameObject("SceneEventBus");
            sceneEventBusGO.AddComponent<SceneEventBus>();
        }

        // Cache references
        gameManager = GameInitiator.Instance.GameManager;
        inputService = ServiceLocator.Get<IInputService>();
        soundService = ServiceLocator.Get<ISoundService>();
        playerManager = ServiceLocator.Get<IPlayerManager>();
        // Register gameplay services
        RegisterServices();

        // Tutorial setup
        if (!playerManager.playerService.GetPlayerData().completedTutorial)
        {
            GenerateTutorialCharacters();
        }

        // Initialize subsystems
        gameplayUI.Initialize(playerManager.playerService);
        enemyManager.Initialize();
        waveManager.Initialize();
        parallaxBackground.Initialize();

        // Initialize state machine
        stateMachine = new StateMachine();
        startState = new GameplayStartState(this);
        playingState = new GameplayPlayingState(this);
        reviveState = new GameplayReviveState(this);
        endState = new GameplayEndState(this);

        isInitialized = true;
        Debug.Log("[GameplayManager] Initialized");
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    private void RegisterServices()
    {
        // Use DI composition root for gameplay services
        GameplayCompositionRoot.Configure(
            GameServices.Container,
            enemyManager,
            waveManager,
            lootManager,
            followerManager,
            damageNumberService,
            gameplayUI
        );
    }
    public UniTask SetState(GameplayState newState)
    {
        if (CurrentState == newState) return UniTask.CompletedTask;

        CurrentState = newState;

        // Use state machine for state transitions
        switch (CurrentState)
        {
            case GameplayState.Start:
                stateMachine.ChangeState(startState);
                break;

            case GameplayState.Playing:
                stateMachine.ChangeState(playingState);
                break;

            case GameplayState.Revive:
                stateMachine.ChangeState(reviveState);
                break;

            case GameplayState.End:
                stateMachine.ChangeState(endState);
                break;
        }

        return UniTask.CompletedTask;
    }

    // Internal methods called by states
    public void SetGameActive(bool active) => IsGameActive = active;

    public void SetupLevelInternal()
    {
        var currentLevel = ServiceLocator.Get<ILevelManager>().activeLevel;

        parallaxBackground.SetupParallaxLayerMaterial(currentLevel.background);
        waveManager.SetWaveConfig(currentLevel.waveSet.waves);
        soundService.Play(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);
        gameplayUI.StartStateSetup();

        InitializeTeams();
    }

    public void HandleTeamChangeInternal() => HandleTeamChange();

    public void StartWaveInternal()
    {
        if (waveManager.currentWave == null)
            waveManager.StartNextWave();
        else
            waveManager.PauseWave(false);
    }

    public void PauseGameplayInternal()
    {
        enemyManager.ResetTargets();
        waveManager.PauseWave(true);
    }

    public void ResumeGameplayInternal()
    {
        waveManager.PauseWave(false);
    }

    public void EndGameplayInternal()
    {
        enemyManager.KillAllEnemies(silent: true);
        MarkTutorialComplete();
    }

    private void SetupLevel()
    {
        var currentLevel = ServiceLocator.Get<ILevelManager>().activeLevel;

        parallaxBackground.SetupParallaxLayerMaterial(currentLevel.background);
        waveManager.SetWaveConfig(currentLevel.waveSet.waves);
        soundService.Play(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);
        gameplayUI.StartStateSetup();

        InitializeTeams();
    }

    private void InitializeTeams()
    {
        var teams = ServiceLocator.Get<ITeamManager>().GetActiveTeam();

        foreach (var team in teams)
        {
            deadTeams[team.GetData().teamID] = false;
        }

        activeTeamID = teams[0].GetData().teamID;
        HandleTeamChange();
    }

    private void HandleTeamChange()
    {
        if (previousTeamID == activeTeamID) return;

        previousTeamID = activeTeamID;

        var teams = ServiceLocator.Get<ITeamManager>().GetActiveTeam();
        var members = teams[0].GetMembers();

        var battleStates = members
            .Select(m => new CharacterBattleState(new CharacterService(m)))
            .ToList();

        var followerObjects = followerManager.Initialize(battleStates);
        playerGameplayManager.Initialize(followerObjects, inputService);
        gameplayUI.SetupCharacterUI(battleStates);
        followerManager.SwitchTo(0);
    }

    private void MarkTutorialComplete()
    {
        var playerData = playerManager.playerService.GetPlayerData();
        if (!playerData.completedTutorial)
        {
            playerData.completedTutorial = true;
        }
    }

    public void TriggerEndGame() => SetState(GameplayState.End).Forget();

    private void StartWave()
    {
        if (waveManager.currentWave == null)
            waveManager.StartNextWave();
        else
            waveManager.PauseWave(false);
    }

    public void SetDeadTeam(string teamID, bool isDead)
    {
        if (!deadTeams.ContainsKey(teamID)) return;

        deadTeams[teamID] = isDead;
        OnDeadTeamUpdated?.Invoke();
    }

    public bool IsTeamDead(string teamID) => deadTeams.TryGetValue(teamID, out var isDead) && isDead;
    private void GenerateTutorialCharacters()
    {
        var teamManager = ServiceLocator.Get<ITeamManager>();
        var characters = ServiceLocator.Get<ICharacterManager>().ownedCharacters;

        teamManager.IncreaseMaxTeam();

        CreateTutorialTeam(teamManager, characters);
        CreateTutorialTeam(teamManager, characters);

        if (GameInitiator.Instance.isDevelopment)
        {
            ServiceLocator.Get<ILevelManager>().activeLevel = tutorialLevel;
        }

        Debug.Log("[GameplayManager] Tutorial characters generated");
    }

    private void CreateTutorialTeam(ITeamManager teamManager, List<CharacterData> characters)
    {
        var teamID = teamManager.CreateTeam();

        for (int i = 0; i < 4; i++)
        {
            var character = CharacterFactory.CreateTestCharacter();
            characters.Add(character);
            teamManager.AssignedCharacterToSlot(teamID, i, character);
        }

        teamManager.SetActiveTeam(teamID);
    }
}
