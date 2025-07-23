using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
    public GameObject loaderCanvas;
    public async UniTask Intialize()
    {
        if (_isInitialize) return;
        loaderCanvas = GameManager.Instance.LevelManager.loaderCanva;
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
    public void SetState(GameState newState)
    {
        if (newState == Currentstate) return;
        Currentstate = newState;
        Debug.Log($"[GameStateManager] State changed to: {newState}");
        OnStateChanged?.Invoke(newState);
        switch (newState)
        {
            case GameState.MainMenu:
                SceneLoader.LoadScene("MainMenu", loaderCanvas);
                break;

            case GameState.Gameplay:
                SceneLoader.LoadScene("Gameplay", loaderCanvas);
                break;

            case GameState.Credits:
                SceneLoader.LoadScene("Credits", loaderCanvas);
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
