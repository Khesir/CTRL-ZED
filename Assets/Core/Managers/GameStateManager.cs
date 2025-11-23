using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    None,
    Initial,         // App starts
    TitleScreen,     // Splash or logo scene
    MainMenu,        // Menu logic
    Gameplay,        // Actual gameplay scene
    Loading,          // Internal state (used to transition)
    Credits,
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState Currentstate { get; private set; } = GameState.None;
    public event Action<GameState> OnStateChanged;
    private string _sceneToLoadAfterLoading;
    public bool _isInitialize = false;
    public UIManager uIManager;
    public async UniTask Intialize()
    {
        if (_isInitialize) return;

        Debug.Log("[GameStateManager] Waiting for loaderCanva...");

        uIManager = GameInitiator.Instance.UIManager;
        _isInitialize = true;
        await UniTask.CompletedTask;
    }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public async UniTask SetState(GameState newState)
    {
        if (GameInitiator.Instance.isDevelopment)
            ServiceLocator.Get<IPlayerDataManager>().AutoSaveTrigger();

        if (newState == Currentstate)
        {
            Debug.LogWarning($"[GameStateManager] Tried to set state to {newState}, but it was already the current state. Scene load skipped.");
            return;
        }

        string targetScene = GameStateUtils.GetSceneNameFromState(newState);
        if (SceneManager.GetActiveScene().name == targetScene)
        {
            Debug.Log($"[GameStateManager] Already in {targetScene}, skipping scene load.");
            Currentstate = newState;
            OnStateChanged?.Invoke(newState);
            return;
        }

        Currentstate = newState;
        Debug.Log($"[GameStateManager] State changed to: {newState}");
        OnStateChanged?.Invoke(newState);
        switch (newState)
        {
            case GameState.MainMenu:
                await SceneLoader.LoadScene("MainMenu", uIManager);
                break;

            case GameState.Gameplay:
                await SceneLoader.LoadScene("Gameplay", uIManager);
                break;

            case GameState.Credits:
                await SceneLoader.LoadScene("Credits", uIManager);
                break;

            case GameState.Loading:
                // Optionally use for loading screen scene
                break;
        }
    }
    public void SetSceneToLoadAfterLoading(string sceneName)
    {
        _sceneToLoadAfterLoading = sceneName;
    }
    public async UniTask HandleSceneLoading()
    {
        // Simulate prep work (load managers, data, etc.)
        await UniTask.Delay(1000);

        if (!string.IsNullOrEmpty(_sceneToLoadAfterLoading))
            await SceneManagerService.FinalizeLoadingTargetScene(_sceneToLoadAfterLoading);
    }
}
