using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerDataManager PlayerDataManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public TeamManager TeamManager { get; private set; }
    public CharacterManager CharacterManager { get; private set; }
    public AntiVirusManager AntiVirusManager { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public StatusEffectManager StatusEffectManager;
    public GameplayActiveStatusEffect activeStatusEffect;


    [Header("Manager Prefabs / References")]
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private AntiVirusManager antiVirusManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StatusEffectManager statusEffectManager;
    [SerializeField] private LevelData tutorialLevel;
    public bool isGameActive;
    public bool _isInitialized = false;
    public bool skipTutorial = false;
    public bool isInTutorial = false;

    [Header("Do not Change Anything")]
    [SerializeField] private SaveData saveData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public async UniTask Initialize()
    {
        if (_isInitialized) return;

        // Initialize the Managers and game systems
        // Setup basic manager references (no save data yet)
        PlayerDataManager = playerDataManager;
        PlayerManager = playerManager;
        CharacterManager = characterManager;
        TeamManager = teamManager;
        AntiVirusManager = antiVirusManager;
        LevelManager = levelManager;
        StatusEffectManager = statusEffectManager;
        // Set Initial game state
        isGameActive = false;

        Debug.Log("[GameManager] Game Manager Initialized");
        _isInitialized = true;
        await UniTask.CompletedTask;
        if (!skipTutorial)
        {
            TutorialTrigger().Forget();
        }
    }

    private async UniTaskVoid TutorialTrigger()
    {
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);

        await UniTask.WaitUntil(() => GameInitiator.Instance.GameStateManager.Currentstate == GameState.MainMenu);
        if (GameInitiator.Instance.GameStateManager.Currentstate == GameState.MainMenu && !playerManager.playerService.GetPlayerData().completedTutorial)
        {
            levelManager.activeLevel = tutorialLevel;
            GameInitiator.Instance.SwitchStates(GameState.Gameplay);
            isInTutorial = true;
        }
    }
    public bool HandleTutorial()
    {
        if (isInTutorial)
        {
            skipTutorial = true;
            isInTutorial = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
