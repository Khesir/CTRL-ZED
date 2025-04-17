using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // [Header("Managers")]
    public InventoryManager InventoryManager { get; private set; }
    public CharacterManager CharacterManager { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    // [Header("Prefabs / References")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private PlayerManager playerManager;
    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public async UniTask InitializeManagerAsync()
    {
        InventoryManager = Instantiate(inventoryManager);
        CharacterManager = Instantiate(characterManager);
        PlayerManager = Instantiate(playerManager);
        await UniTask.WhenAll(
            // InventoryManager.Initialize();
            CharacterManager.Initialize(),
            PlayerManager.Initialize()
        );
    }
}
