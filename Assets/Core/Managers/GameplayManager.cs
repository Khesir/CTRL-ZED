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
    [SerializeField] private IInputService inputService;

    [Header("Gameplay References")]
    public FollowerManager followerManager;
    public PlayerGameplayManager playerGameplayManager;
    public ParallaxBackground parallaxBackground;
    public FollowerSpawn spawn;
    public GameplayUIController gameplayUI;
    public EnemySpawner spawner;
    public SquadLevelManager squadLevelManager;
    public DamageNumberController damageNumberController;

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
        squadLevelManager.Setup(100);
        Debug.Log("[GameplayManager] Gameplay Manager Initialized");
        _isInitialized = true;
        await UniTask.CompletedTask;
    }

    public async UniTask Setup()
    {
        List<TeamService> team = GameManager.Instance.TeamManager.GetActiveTeam();
        List<CharacterService> members = team[0].GetMembers();
        List<CharacterBattleState> battleStates = members.Select(m => new CharacterBattleState(m)).ToList();

        List<GameObject> GO = followerManager.Initialize(battleStates);
        playerGameplayManager.Initialize(GO, inputService);

        // Wait for one frame to make sure followers are instantiated
        await UniTask.NextFrame();
        await spawner.Initialize(gameManager.LevelManager.activeLevel.waves);
        followerManager.SwitchTo(0);
        isGameActive = true;
        Debug.Log("[GameplayManager] Gameplay Manager is now Active");

    }

    // public void AddFollower(Follower data)
    // {
    //     followers.Add(data);
    // }
    // private void IsDead(int index)
    // {
    //     var character = followers[index].GetComponent<PlayerController>().playerData;
    //     if (character.isDead)
    //     {
    //         return;
    //     }
    // }
    // public int IsAvailable()
    // {
    //     for (int i = 0; i < followers.Count; i++)
    //     {
    //         var controller = followers[i].GetComponent<PlayerController>().playerData;

    //         if (!controller.isDead)
    //         {
    //             return i;
    //         }
    //         else
    //         {
    //             continue;
    //         }
    //     }
    //     return -1;
    // }
    // public void SetTarget()
    // {
    //     currentFollowerIndex = -1;
    //     globalTargetPlayer = null;
    //     switchUser?.Invoke();
    // }
}
