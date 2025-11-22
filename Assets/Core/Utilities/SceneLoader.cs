using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static async UniTask LoadScene(string sceneName, UIManager canvas)
    {
        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        await ServiceLocator.Get<ISoundService>().FadeOut(SoundCategory.BGM);
        canvas.ShowLoading(true);

        var loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        // Wait until loading reaches 90% (Unity's async loading stops here)
        while (loadOperation.progress < 0.9f)
        {
            await Task.Delay(100);
            await UniTask.Yield();
        }
        await Task.Delay(1000);
        Debug.Log($"[SceneLoader] Scene {sceneName} loaded. Activating...");

        // Now activate the scene
        loadOperation.allowSceneActivation = true;
        // Optionally wait one more frame to let it finish
        await UniTask.Yield();
        canvas.ShowLoading(false);
        Debug.Log($"[SceneLoader] Scene {sceneName} loaded successfully.");

    }
}