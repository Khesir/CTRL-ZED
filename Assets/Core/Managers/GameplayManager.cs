using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public enum GameplayState { None, Start, Playing, Revive, End }
public enum GameplayEndGameState { DeathOnTimer, LevelComplete }
// This is the brain for this scene.
public class GameplayManager : MonoBehaviour, IGameplayManager
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

    [SerializeField] private Button SkipButton;
    // IGameplayManager implementation
    [SerializeField] public bool IsGameActive { get; set; } = false;
    public GameplayEndGameState EndGameState { get; set; }
    public GameplayState CurrentState { get; private set; } = GameplayState.None;

    public string ActiveTeamID
    {
        get => activeTeamID;
        set
        {
            activeTeamID = value;
            HandleTeamChange();
        }
    }

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

        // Create SceneEventBus EARLY so other scripts can subscribe in OnEnable()
        EnsureSceneEventBus();
    }

    private void EnsureSceneEventBus()
    {
        if (FindAnyObjectByType<SceneEventBus>() == null)
        {
            var sceneEventBusGO = new GameObject("SceneEventBus");
            sceneEventBusGO.AddComponent<SceneEventBus>();
        }
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        Initialize();
        await SetState(GameplayState.Start);
    }
    private void Update()
    {
        stateMachine?.Update();
    }
    private void Initialize()
    {
        if (isInitialized) return;

        // Cache references
        gameManager = GameInitiator.Instance.GameManager;
        inputService = ServiceLocator.Get<IInputService>();
        soundService = ServiceLocator.Get<ISoundService>();
        playerManager = ServiceLocator.Get<IPlayerManager>();

        // Use DI composition root for gameplay services
        GameplayCompositionRoot.Configure(
            GameServices.Container,
            this,
            enemyManager,
            waveManager,
            lootManager,
            followerManager,
            damageNumberService,
            gameplayUI
        );
        // Tutorial setup
        if (!playerManager.playerService.GetPlayerData().completedTutorial)
        {
            GenerateTutorialCharacters();
            SkipButton.gameObject.SetActive(true);
            SkipButton.onClick.RemoveAllListeners();
            SkipButton.onClick.AddListener(SkipWave);
        }

        // Initialize subsystems
        gameplayUI.Initialize(playerManager.playerService);
        enemyManager.Initialize();
        waveManager.Initialize();
        parallaxBackground.Initialize();
        lootManager.Initialize();
        // Initialize state machine
        stateMachine = new StateMachine();
        startState = new GameplayStartState(this);
        playingState = new GameplayPlayingState(this);
        reviveState = new GameplayReviveState(this);
        endState = new GameplayEndState(this);
        isInitialized = true;

        SceneEventBus.Subscribe<GameplayPlayerDeathEvent>(HandleDeath);
        SceneEventBus.Subscribe<WaveCompletedEvent>(HandleWaveComplete);
        Debug.Log("[GameplayManager] Initialized");
    }
    #region  Gameplay Events
    private void HandleDeath(GameplayPlayerDeathEvent evt)
    {
        // reviveCount
        gameplayUI.revivePanel.SetDisplay(true);
    }
    private void HandleWaveComplete(WaveCompletedEvent evt)
    {
        if (!evt.IsLastWave)
        {
            SetState(GameplayState.Start); // Reset with new set
        }
        else
        {
            EndGameState = GameplayEndGameState.LevelComplete;
            SetState(GameplayState.End); // Level Complete
        }
    }
    #endregion

    #region Statemachine
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
    #endregion

    #region Internal Methods by states
    // Internal methods called by states
    public void SetupLevelInternal()
    {
        var currentLevel = ServiceLocator.Get<ILevelManager>().activeLevel;

        parallaxBackground.SetupParallaxLayerMaterial(currentLevel.background);
        waveManager.SetWaveConfig(currentLevel.waveSet.waves);
        soundService.Play(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);
        gameplayUI.StartStateSetup();

        var teams = ServiceLocator.Get<ITeamManager>().GetActiveTeam();

        foreach (var team in teams)
        {
            deadTeams[team.GetData().teamID] = false;
        }

        activeTeamID = teams[0].GetData().teamID;
        HandleTeamChange();
    }
    public void HandleTeamChangeInternal() => HandleTeamChange();
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

        // Apply any purchased antivirus buffs to all characters in the scene
        ServiceLocator.Get<IStatusEffectManager>()?.ApplyAllStoredBuffsToScene();
    }
    public void StartWaveInternal()
    {
        if (waveManager.currentWave == null)
            waveManager.StartNextWave();
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
        gameplayUI.HandleEndGamePanel(EndGameState);
        MarkTutorialComplete();
        // Clear all antivirus buffs when gameplay ends (silently to avoid destroyed object exceptions)
        ServiceLocator.Get<IStatusEffectManager>()?.ClearAllBuffs(invokeEvent: false);
        Debug.Log("[GameplayManager] Cleared all antivirus buffs silently after gameplay ended");
    }


    private void MarkTutorialComplete()
    {
        var playerData = playerManager.playerService.GetPlayerData();
        if (!playerData.completedTutorial)
        {
            playerData.completedTutorial = true;
        }
        ServiceLocator.Get<GameInitiator>().IntroViewed();
    }
    #endregion

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

    #region TutorialGamePlay
    private void GenerateTutorialCharacters()
    {
        var teamManager = ServiceLocator.Get<ITeamManager>();
        var characters = ServiceLocator.Get<ICharacterManager>().ownedCharacters;

        teamManager.IncreaseMaxTeam();

        CreateTutorialTeam(teamManager, characters);
        CreateTutorialTeam(teamManager, characters);

        // activeLevel is now set by GameInitiator in dev mode

        Debug.Log("[GameplayManager] Tutorial characters generated");
    }

    private void CreateTutorialTeam(ITeamManager teamManager, List<CharacterData> characters)
    {
        var teamID = teamManager.CreateTeam();
        List<CharacterConfig> possibleConfigs = ServiceLocator.Get<ICharacterManager>().characterTemplates;
        var config = possibleConfigs[UnityEngine.Random.Range(0, possibleConfigs.Count)];

        for (int i = 0; i < 4; i++)
        {
            var character = CharacterFactory.CreateFromShop(config);
            characters.Add(character);
            teamManager.AssignedCharacterToSlot(teamID, i, character);
        }

        teamManager.SetActiveTeam(teamID);
    }
    #endregion

    [ContextMenu("Wave Complete")]
    private void SkipWave()
    {
        EndGameplayInternal();
    }
}
