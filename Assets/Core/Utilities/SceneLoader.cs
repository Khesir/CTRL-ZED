using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneLoader
{
    public static async void LoadScene(string sceneName)
    {
        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");

        var loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            await UniTask.Yield();
        }

        Debug.Log($"[SceneLoader] Scene {sceneName} loaded.");
    }
}