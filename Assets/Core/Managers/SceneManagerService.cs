using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerService
{
    public static string CurrentSceneName => SceneManager.GetActiveScene().name;

    public static event Action<string> OnSceneLoaded;
    public static event Action<string> OnSceneUnloaded;

    private static string _loadingSceneName = "LoadingScene";

    /// <summary>
    /// Immediately loads a scene (synchronously). Use with caution.
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        OnSceneLoaded?.Invoke(sceneName);
    }

    /// <summary>
    /// Loads a scene asynchronously and waits until it’s ready.
    /// </summary>
    public static async UniTask LoadSceneAsync(string sceneName)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            await UniTask.Yield();
        }

        OnSceneLoaded?.Invoke(sceneName);
    }

    /// <summary>
    /// Unloads a scene asynchronously (additive scenes).
    /// </summary>
    public static async UniTask UnloadSceneAsync(string sceneName)
    {
        var operation = SceneManager.UnloadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            await UniTask.Yield();
        }

        OnSceneUnloaded?.Invoke(sceneName);
    }

    /// <summary>
    /// Loads a scene via a loading transition scene (intermediate).
    /// </summary>
    public static async UniTask LoadWithLoadingScreen(string targetScene)
    {
        GameStateManager.Instance.SetSceneToLoadAfterLoading(targetScene);
        await LoadSceneAsync(_loadingSceneName);
    }

    /// <summary>
    /// Used by the loading scene to load the real scene after prep is done.
    /// </summary>
    public static async UniTask FinalizeLoadingTargetScene(string sceneToLoad)
    {
        await LoadSceneAsync(sceneToLoad);
    }
}
