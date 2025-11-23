using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class LoopingBackgroundVideo : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;

    [Header("Fallback")]
    [SerializeField] private Image fallbackImage;

    [Header("Settings")]
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private float volume = 0f; // Default muted for BG video

    private RenderTexture renderTexture;
    private bool hasVideo;

    private void Awake()
    {
        hasVideo = videoClip != null && videoPlayer != null && videoDisplay != null;

        if (hasVideo)
        {
            // Enable video display if it was disabled in inspector
            if (!videoDisplay.gameObject.activeSelf)
                videoDisplay.gameObject.SetActive(true);

            SetupRenderTexture();
            ConfigureVideoPlayer();
            ShowVideo();
        }
        else
        {
            // Make sure video display stays hidden if no video
            if (videoDisplay != null)
                videoDisplay.gameObject.SetActive(false);

            ShowFallback();
        }
    }

    private void Start()
    {
        if (playOnAwake && hasVideo)
        {
            Play();
        }
    }

    private void ShowVideo()
    {
        if (videoDisplay != null)
            videoDisplay.gameObject.SetActive(true);

        if (fallbackImage != null)
            fallbackImage.gameObject.SetActive(false);
    }

    private void ShowFallback()
    {
        if (videoDisplay != null)
            videoDisplay.gameObject.SetActive(false);

        if (fallbackImage != null)
            fallbackImage.gameObject.SetActive(true);

        Debug.Log("[LoopingBackgroundVideo] No video clip assigned, showing fallback image.");
    }

    private void SetupRenderTexture()
    {

        // Create render texture matching video dimensions or default
        int width = videoClip != null ? (int)videoClip.width : 1920;
        int height = videoClip != null ? (int)videoClip.height : 1080;

        renderTexture = new RenderTexture(width, height, 0);
        renderTexture.Create();

        videoPlayer.targetTexture = renderTexture;
        videoDisplay.texture = renderTexture;
    }

    private void ConfigureVideoPlayer()
    {
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;
        videoPlayer.isLooping = true;
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
        videoPlayer.SetDirectAudioVolume(0, volume);
    }

    public void Play()
    {
        if (!hasVideo)
        {
            Debug.LogWarning("[LoopingBackgroundVideo] No video clip assigned, showing fallback.");
            ShowFallback();
            return;
        }

        videoPlayer.Play();
    }

    public void Pause()
    {
        if (videoPlayer != null)
            videoPlayer.Pause();
    }

    public void Stop()
    {
        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    public void SetVideo(VideoClip clip)
    {
        videoClip = clip;
        hasVideo = clip != null && videoPlayer != null && videoDisplay != null;

        if (hasVideo)
        {
            videoPlayer.clip = clip;

            // Recreate render texture for new video dimensions
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
            SetupRenderTexture();
            ShowVideo();
            Play();
        }
        else
        {
            ShowFallback();
        }
    }

    public void SetVolume(float vol)
    {
        volume = Mathf.Clamp01(vol);
        if (videoPlayer != null)
            videoPlayer.SetDirectAudioVolume(0, volume);
    }

    public bool IsPlaying => videoPlayer != null && videoPlayer.isPlaying;

    private void OnEnable()
    {
        if (playOnAwake && hasVideo && !IsPlaying)
        {
            Play();
        }
    }

    private void OnDisable()
    {
        Pause();
    }

    private void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release();
            Destroy(renderTexture);
        }
    }
}
