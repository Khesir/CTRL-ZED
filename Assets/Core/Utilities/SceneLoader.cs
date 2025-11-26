using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private const float MinLoadingDisplayTime = 2f; // Minimum seconds to show loading screen

    public static async UniTask LoadScene(string sceneName, UIManager canvas)
    {
        Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
        await ServiceLocator.Get<ISoundService>().FadeOut(SoundCategory.BGM);

        bool isInTutorial = GameInitiator.Instance != null && GameInitiator.Instance.isInTutorial;

        if (isInTutorial && canvas.HasTutorialVideo && !GameInitiator.Instance.introViewed)
        {
            canvas.ShowLoading(true);

            // Tutorial flow: Video first, then load scene
            Debug.Log("[SceneLoader] Playing tutorial video...");

            await canvas.PlayTutorialVideo();

            float loadingStartTime = Time.unscaledTime;

            var loadOperation = SceneManager.LoadSceneAsync(sceneName);
            loadOperation.allowSceneActivation = false;

            // Wait until scene is ready
            while (loadOperation.progress < 0.9f)
            {
                await UniTask.Yield();
            }

            // Ensure minimum loading display time
            float elapsedTime = Time.unscaledTime - loadingStartTime;
            if (elapsedTime < MinLoadingDisplayTime)
            {
                float remainingTime = MinLoadingDisplayTime - elapsedTime;
                Debug.Log($"[SceneLoader] Waiting {remainingTime:F1}s for minimum loading time...");
                await UniTask.Delay((int)(remainingTime * 1000), ignoreTimeScale: true);
            }

            Debug.Log($"[SceneLoader] Scene {sceneName} loaded. Activating...");
            loadOperation.allowSceneActivation = true;
            await UniTask.Yield();
            canvas.ShowLoading(false);
        }
        else
        {
            // Normal flow: Just load with loading screen
            canvas.ShowLoading(true);

            var loadOperation = SceneManager.LoadSceneAsync(sceneName);
            loadOperation.allowSceneActivation = false;

            while (loadOperation.progress < 0.9f)
            {
                await UniTask.Yield();
            }

            await UniTask.Delay(1000, ignoreTimeScale: true);

            Debug.Log($"[SceneLoader] Scene {sceneName} loaded. Activating...");
            loadOperation.allowSceneActivation = true;
            await UniTask.Yield();
            canvas.ShowLoading(false);
        }

        Debug.Log($"[SceneLoader] Scene {sceneName} loaded successfully.");
    }
}