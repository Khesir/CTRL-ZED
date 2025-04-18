using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public InventoryManager InventoryManager { get; private set; }
    public CharacterManager CharacterManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    [Header("Manager Prefabs / References")]
    // [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private PlayerManager playerManager;
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
        PlayerManager = playerManager;
        await PlayerManager.Initialize();

        CharacterManager = characterManager;
        await CharacterManager.Initialize();
    }
}
