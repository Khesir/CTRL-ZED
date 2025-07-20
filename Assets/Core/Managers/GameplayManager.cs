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
    [Header("Follower System")]
    public GameObject followerPrefab;
    public Transform Spawnpoint;
    [SerializeField] public List<Follower> followers = new List<Follower>();
    [SerializeField] private int currentFollowerIndex = 0;
    public Transform globalTargetPlayer;
    public CinemachineVirtualCamera virtualCamera;
    public event Action switchUser;
    public bool isGameActive;
    public bool _isInitialized = false;
    [Header("Gameplay References")]
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
    public async UniTask Initialize()
    {
        if (_isInitialized) return;

        await parallaxBackground.Initialize();
        squadLevelManager.Setup(SquadMaxLevel);
        // Set Initial game state
        isGameActive = false;

        Debug.Log("[GameplayManager] Gameplay Manager Initialized");
        _isInitialized = true;
        await UniTask.CompletedTask;
    }
    public async UniTask Setup()
    {
        gameManager = GameManager.Instance;
        if (followers.Count == 0)
        {
            List<TeamService> team = GameManager.Instance.TeamManager.GetActiveTeam();
            List<CharacterService> members = team[0].GetMembers();
            var battleStates = members.Select(m => new CharacterBattleState(m)).ToList();
            spawn.Setup(battleStates);
        }
        SwitchControlledFollower(currentFollowerIndex);
        await spawner.Initialize(gameManager.LevelManager.activeLevel.waves);
        parallaxBackground.SetupParallaxLayerMaterial(gameManager.LevelManager.activeLevel.background);
        await UniTask.CompletedTask;
    }
    void Update()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i != currentFollowerIndex)
                {
                    IsDead(i);
                    SwitchControlledFollower(i);
                }
            }
        }
    }
    public void SwitchControlledFollower(int newIndex)
    {
        if (newIndex >= 0 && newIndex < followers.Count)
        {

            var newLeader = followers[newIndex];
            currentFollowerIndex = newIndex;
            globalTargetPlayer = newLeader.transform;
            virtualCamera.Follow = newLeader.transform;

            // Handles UI
            var characterService = newLeader.GetComponent<PlayerController>().playerData;
            Instance.gameplayUI.characterListUI.hotbar1.GetComponent<CharacterDetails>().Initialize(characterService);
            for (int i = 0; i < followers.Count; i++)
            {
                if (i == newIndex)
                {
                    followers[i].SetTarget();
                }
                else
                {
                    followers[i].Refresh();
                }
            }
            switchUser?.Invoke();
            Debug.Log($"Now controlling follower {currentFollowerIndex + 1}");
        }
    }
    public void AddFollower(Follower data)
    {
        followers.Add(data);
    }
    private void IsDead(int index)
    {
        var character = followers[index].GetComponent<PlayerController>().playerData;
        if (character.isDead)
        {
            return;
        }
    }
    public int IsAvailable()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            var controller = followers[i].GetComponent<PlayerController>().playerData;

            if (!controller.isDead)
            {
                return i;
            }
            else
            {
                continue;
            }
        }
        return -1;
    }
    public void SetTarget()
    {
        currentFollowerIndex = -1;
        globalTargetPlayer = null;
        switchUser?.Invoke();
    }
}
