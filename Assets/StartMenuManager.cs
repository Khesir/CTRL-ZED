using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance { get; private set; }
    [Header("Menu References")]

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
        Debug.Log("[StartMenuManager] Initializing Menu Manager");
        isGameActive = false;
        Setup();
        PlayMusic();
        _isInitialized = true;
        Debug.Log("[StartMenuManager]  StartMenuManager Manager Initialized");
        await UniTask.CompletedTask;
    }
    private void Setup()
    {
        Debug.Log("[StartMenuManager] Setup Finished");
    }
    private void PlayMusic()
    {
        SoundManager.PlaySound(SoundCategory.BGM, SoundType.BGM_Start, 0.5f);
        Debug.Log("[StartMenuManager] Playing BGM");
    }
}
