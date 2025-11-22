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
    private bool isInitialized;

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

        // Cache references
        gameManager = GameInitiator.Instance.GameManager;
        inputService = ServiceLocator.Get<IInputService>();
        soundService = ServiceLocator.Get<ISoundService>();

        // Register gameplay services
        RegisterServices();

        // Tutorial setup
        if (!gameManager.PlayerManager.playerService.GetPlayerData().completedTutorial)
        {
            GenerateTutorialCharacters();
        }

        // Initialize subsystems
        gameplayUI.Initialize(gameManager.PlayerManager.playerService);
        enemyManager.Initialize();
        waveManager.Initialize();
        parallaxBackground.Initialize();

        isInitialized = true;
        Debug.Log("[GameplayManager] Initialized");
    }

    private void RegisterServices()
    {
        ServiceLocator.Register<IEnemyManager>(enemyManager);
        ServiceLocator.Register<ILootManager>(lootManager);
        ServiceLocator.Register<IFollowerManager>(followerManager);
        ServiceLocator.Register<IWaveManager>(waveManager);
        ServiceLocator.Register<IDamageNumberService>(damageNumberService);
    }
    public async UniTask SetState(GameplayState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        switch (CurrentState)
        {
            case GameplayState.Start:
                await EnterStartState();
                break;

            case GameplayState.Playing:
                await EnterPlayingState();
                break;

            case GameplayState.Revive:
                EnterReviveState();
                break;

            case GameplayState.End:
                EnterEndState();
                break;
        }
    }

    private async UniTask EnterStartState()
    {
        if (!IsGameActive) SetupLevel();

        if (gameplayUI != null)
            await gameplayUI.StartStateUIAnimation();

        IsGameActive = true;
    }

    private async UniTask EnterPlayingState()
    {
        HandleTeamChange();

        if (gameplayUI != null)
            await gameplayUI.PlayingStateUIAnimation();

        StartWave();
    }

    private void EnterReviveState()
    {
        enemyManager.ResetTargets();
        waveManager.PauseWave(true);
    }

    private void EnterEndState()
    {
        enemyManager.KillAllEnemies(silent: true);
        // gameplayUI?.HandleEndGamePanel(EndGameState);
        MarkTutorialComplete();
    }

    private void SetupLevel()
    {
        var currentLevel = gameManager.LevelManager.activeLevel;

        parallaxBackground.SetupParallaxLayerMaterial(currentLevel.background);
        waveManager.SetWaveConfig(currentLevel.waveSet.waves);
        soundService.Play(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);
        gameplayUI.StartStateSetup();

        InitializeTeams();
    }

    private void InitializeTeams()
    {
        var teams = gameManager.TeamManager.GetActiveTeam();

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

        var teams = gameManager.TeamManager.GetActiveTeam();
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
        var playerData = gameManager.PlayerManager.playerService.GetPlayerData();
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
        var teamManager = gameManager.TeamManager;
        var characters = gameManager.CharacterManager.ownedCharacters;

        teamManager.IncreaseMaxTeam();

        CreateTutorialTeam(teamManager, characters);
        CreateTutorialTeam(teamManager, characters);

        if (GameInitiator.Instance.isDevelopment)
        {
            gameManager.LevelManager.activeLevel = tutorialLevel;
        }

        Debug.Log("[GameplayManager] Tutorial characters generated");
    }

    private void CreateTutorialTeam(TeamManager teamManager, List<CharacterData> characters)
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
