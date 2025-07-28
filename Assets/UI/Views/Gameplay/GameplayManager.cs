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

    public GameManager gameManager;
    public static GameplayManager Instance { get; private set; }
    // [Header("Follower System")]
    // public GameObject followerPrefab;
    // public Transform Spawnpoint;
    // public List<Follower> followers = new List<Follower>();
    // [SerializeField] private int currentFollowerIndex = 0;
    // public Transform globalTargetPlayer;
    // public CinemachineVirtualCamera virtualCamera;
    // public event Action switchUser;
    public bool isGameActive;
    public bool _isInitialized = false;
    [Header("Gameplay References")]
    public FollowerManager followerManager;
    public PlayerGameplayManager playerGameplayManager;
    public ParallaxBackground parallaxBackground;
    public FollowerSpawn spawn;
    public GameplayUIController gameplayUI;
    public EnemySpawner spawner;
    public SquadLevelManager squadLevelManager;
    public DamageNumberController damageNumberController;
    public int SquadMaxLevel;
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
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isGenerated);
        await Initialize();
        await Setup();
    }
    public async UniTask Initialize()
    {
        if (_isInitialized) return;

        // Check all Managers;

        // Initialize data level
        await parallaxBackground.Initialize();
        squadLevelManager.Setup(SquadMaxLevel);
        Debug.Log("[GameplayManager] Gameplay Manager Initialized");
        _isInitialized = true;
        await UniTask.CompletedTask;
    }

    public async UniTask Setup()
    {
        List<TeamService> team = GameManager.Instance.TeamManager.GetActiveTeam();
        List<CharacterService> members = team[0].GetMembers();
        var battleStates = members.Select(m => new CharacterBattleState(m)).ToList();

        spawn.Setup(battleStates); // This handles spawns the GameObject with everything attach including follower system

        // Wait for one frame to make sure followers are instantiated
        await UniTask.NextFrame();
        playerGameplayManager.Initialize(spawn.SpawnedFollowers);
        await spawner.Initialize(gameManager.LevelManager.activeLevel.waves);
        isGameActive = false;
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
