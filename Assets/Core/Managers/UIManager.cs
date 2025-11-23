using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Global Canvases")]
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private Canvas popupCanvas;

    [Header("Video")]
    [SerializeField] private SkippableVideoPlayer videoPlayer;
    [SerializeField] private VideoClip tutorialVideo;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowLoading(bool show)
    {
        if (loadingCanvas != null)
            loadingCanvas.gameObject.SetActive(show);
    }
    public void ShowPopup(GameObject popupPrefab)
    {
        if (popupCanvas != null & popupCanvas != null)
        {
            Instantiate(popupPrefab, popupCanvas.transform);
        }
    }

    public async UniTask PlayTutorialVideo()
    {
        if (videoPlayer == null || tutorialVideo == null)
        {
            Debug.LogWarning("[UIManager] Video player or tutorial video not assigned.");
            return;
        }
        videoPlayer.gameObject.SetActive(true);
        await videoPlayer.PlayVideo(tutorialVideo);
        await UniTask.Delay(2000, ignoreTimeScale: true);
        videoPlayer.gameObject.SetActive(false);
    }

    public bool HasTutorialVideo => videoPlayer != null && tutorialVideo != null;
}
