using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerDataManager PlayerDataManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public TeamManager TeamManager { get; private set; }
    public CharacterManager CharacterManager { get; private set; }
    public List<CharacterConfig> characterTemplates;
    [Header("Manager Prefabs / References")]
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private PlayerDataManager playerDataManager;
    public bool isGameActive;
    public bool _isInitialized = false;
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
        await InitializeManagerAsync();

        // Set Initial game state
        isGameActive = false;

        Debug.Log("[GameManage] Game Manager Initialized");
        _isInitialized = true;
    }
    public async UniTask InitializeManagerAsync()
    {
        // InventoryManager = Instantiate(inventoryManager);
        PlayerDataManager = playerDataManager;
        SaveData saveData = await PlayerDataManager.Initialize();

        await playerManager.Initialize(saveData.playerData);
        await teamManager.Initialize(saveData.teams);
        await characterManager.Initialize(saveData.ownedCharacters);

        PlayerManager = playerManager;
        CharacterManager = characterManager;
        TeamManager = teamManager;
    }
}
