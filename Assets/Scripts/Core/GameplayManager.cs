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
    public GameManager Menu_istance;
    public static GameplayManager Instance { get; private set; }
    public GameObject followerPrefab;
    public Transform Spawnpoint;
    public GameplayUIController gameplayUI;
    [SerializeField] public List<Follower> followers = new List<Follower>();

    [SerializeField] private int currentFollowerIndex = 0;
    public Transform globalTargetPlayer;
    public CinemachineVirtualCamera virtualCamera;
    public FollowerSpawn spawn;
    public event Action switchUser;
    public bool isGameActive;
    public bool _isInitialized = false;
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

        // Initialize the Managers and game systems
        await UniTask.Yield();
        // Set Initial game state
        isGameActive = false;

        Debug.Log("[GameplayManage] Gameplay Manager Initialized");
        _isInitialized = true;
    }
    public async UniTask Setup()
    {
        Menu_istance = GameManager.Instance;
        if (followers.Count == 0)
        {
            TeamService team = GameManager.Instance.TeamManager.GetActiveTeam();
            List<CharacterService> members = team.GetMembers();
            spawn.Setup(members);
        }
        SwitchControlledFollower(currentFollowerIndex);
        await UniTask.CompletedTask;
    }
    void Update()
    {
        Instance.gameplayUI.timer.TriggerTimer(); // Attack Timer
        for (int i = 0; i < followers.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i != currentFollowerIndex)
                {
                    var character = followers[i].GetComponent<PlayerController>().playerData;
                    if (character.IsDead())
                    {
                        return;
                    }
                    SwitchControlledFollower(i);
                    switchUser.Invoke();
                }
            }
        }
    }
    private void SwitchControlledFollower(int newIndex)
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

            Debug.Log($"Now controlling follower {currentFollowerIndex + 1}");
        }
    }
    public void AddFollower(Follower data)
    {
        followers.Add(data);
    }
    private void SetupUI()
    {
        TeamService team = GameManager.Instance.TeamManager.GetActiveTeam();
    }
}
