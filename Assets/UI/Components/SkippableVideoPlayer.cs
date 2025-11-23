using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SkippableVideoPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button skipButton;

    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.3f;

    private bool isPlaying;
    private bool skipRequested;
    private RenderTexture renderTexture;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (skipButton != null)
            skipButton.onClick.AddListener(Skip);

        gameObject.SetActive(false);
    }

    public async UniTask PlayVideo(VideoClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[SkippableVideoPlayer] No video clip provided.");
            return;
        }

        await PlayVideoInternal(clip);
    }

    public async UniTask PlayVideo(string videoPath)
    {
        if (string.IsNullOrEmpty(videoPath))
        {
            Debug.LogWarning("[SkippableVideoPlayer] No video path provided.");
            return;
        }

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoPath);

        await PlayVideoInternal(null);
    }

    private async UniTask PlayVideoInternal(VideoClip clip)
    {
        isPlaying = true;
        skipRequested = false;

        // Setup render texture
        renderTexture = new RenderTexture(1920, 1080, 0);
        videoPlayer.targetTexture = renderTexture;
        videoDisplay.texture = renderTexture;

        if (clip != null)
        {
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.clip = clip;
        }

        // Show UI
        gameObject.SetActive(true);

        // Fade in
        await FadeCanvasGroup(0f, 1f, fadeInDuration);

        // Prepare and play video
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            await UniTask.Yield();
        }

        videoPlayer.Play();
        Debug.Log("[SkippableVideoPlayer] Video started playing.");

        // Wait for video to finish or skip button pressed
        // Check frame count to detect video end (works even when paused)
        while (isPlaying && !skipRequested)
        {
            // Only check for end if video has actually started (frame >= 0)
            if (videoPlayer.frame >= 0 && videoPlayer.frameCount > 0)
            {
                bool videoEnded = videoPlayer.frame >= (long)(videoPlayer.frameCount - 1);
                if (videoEnded && !videoPlayer.isLooping)
                    break;
            }

            await UniTask.Yield();
        }

        // Stop and cleanup
        videoPlayer.Stop();

        // Fade out
        await FadeCanvasGroup(1f, 0f, fadeOutDuration);

        // Cleanup
        gameObject.SetActive(false);
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
            renderTexture = null;
        }

        isPlaying = false;
        Debug.Log($"[SkippableVideoPlayer] Video ended. Skipped: {skipRequested}");
    }

    private async UniTask FadeCanvasGroup(float from, float to, float duration)
    {
        if (canvasGroup == null) return;

        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            await UniTask.Yield();
        }

        canvasGroup.alpha = to;
    }

    public void Skip()
    {
        skipRequested = true;
        isPlaying = false;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (videoPlayer == null) return;

        if (pauseStatus)
        {
            // App lost focus - pause video
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
        }
        else
        {
            // App regained focus - resume video
            if (isPlaying && videoPlayer.isPaused)
                videoPlayer.Play();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (videoPlayer == null) return;

        if (!hasFocus)
        {
            // App lost focus - pause video
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
        }
        else
        {
            // App regained focus - resume video
            if (isPlaying && videoPlayer.isPaused)
                videoPlayer.Play();
        }
    }

    private void OnDisable()
    {
        isPlaying = false;
        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Stop();
    }
}
