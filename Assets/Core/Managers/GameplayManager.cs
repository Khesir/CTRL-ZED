using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;

public class GameplayManager : MonoBehaviour
{

    public static GameplayManager Instance { get; private set; }
    public bool isGameActive;
    public bool _isInitialized = false;
    [Header("Core References")]
    public GameManager gameManager;
    public GameplayUIController gameplayUI;

    [SerializeField] private IInputService inputService;

    [Header("Gameplay References")]
    public FollowerManager followerManager;
    public PlayerGameplayManager playerGameplayManager;
    public ParallaxBackground parallaxBackground;
    public FollowerSpawn spawn;
    public SquadLevelManager squadLevelManager;
    public DamageNumberController damageNumberController;
    public EnemySpawner spawner;
    public EnemyManager enemyManager;
    public WaveManager waveManager;
    public LootManager lootManager;
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
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        await Initialize();
        await Setup();
    }
    public async UniTask Initialize()
    {
        if (_isInitialized) return;

        // Check all core Managers;
        inputService = GameInitiator.Instance.GetInputService();
        gameManager = GameInitiator.Instance.GetGameManager();
        // Initialize data level
        await parallaxBackground.Initialize();
        // squadLevelManager.Setup(100);
        Debug.Log("[GameplayManager] Gameplay Manager Initialized");
        _isInitialized = true;
        await UniTask.CompletedTask;
    }

    public async UniTask Setup()
    {
        List<TeamService> team = GameManager.Instance.TeamManager.GetActiveTeam();
        List<CharacterData> members = team[0].GetMembers();
        List<CharacterBattleState> battleStates = members.Select(m => new CharacterBattleState(new CharacterService(m))).ToList();

        List<GameObject> GO = followerManager.Initialize(battleStates);
        playerGameplayManager.Initialize(GO, inputService);

        // Wait for one frame to make sure followers are instantiated
        await UniTask.NextFrame();
        waveManager.Initialize(GameManager.Instance.LevelManager.activeLevel);
        followerManager.SwitchTo(0);
        gameplayUI.Initialize(battleStates);
        PlayMusic();
        isGameActive = true;
        enemyManager.Initialize();
        Debug.Log("[GameplayManager] Gameplay Manager is now Active");
    }
    private void PlayMusic()
    {
        SoundManager.PlaySound(SoundCategory.BGM, SoundType.BGM_Gameplay1, 0.5f);
    }
}
