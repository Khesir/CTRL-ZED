using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    [Header("Menu References")]
    public OsExpTop osExpTop;
    public FundsMenuComponent fundsMenuComponent;
    public ResourceUI resourceUI;
    public RepairComponent repairComponent;
    public DrivesMenuComponent drivesMenuComponent;
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
    private async void Start()
    {
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        await Initialize();
    }
    public async UniTask Initialize()
    {
        if (_isInitialized) return;
        Debug.Log("[MenuManager] Initializing Menu Manager");
        isGameActive = false;
        Setup();
        PlayMusic();
        _isInitialized = true;
        Debug.Log("[MenuManager ] Menu Manager Initialized");
        await UniTask.CompletedTask;
    }
    private void Setup()
    {
        osExpTop.Setup();
        fundsMenuComponent.Setup();
        resourceUI.Setup();
        repairComponent.Setup();
        drivesMenuComponent.Setup();
    }
    private void PlayMusic()
    {
        SoundManager.PlaySound(SoundCategory.BGM, SoundType.BGM_MainMenu, 0.5f);
    }
}
