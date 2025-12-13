using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class QuickVideoPlayBtn : MonoBehaviour
{
    [SerializeField] private Button quickButton;
    [Header("Video")]
    [SerializeField] private SkippableVideoPlayer videoPlayer;
    [SerializeField] private VideoClip tutorialVideo;

    void Start()
    {
        if (quickButton != null)
        {
            quickButton.onClick.RemoveAllListeners();
            quickButton.onClick.AddListener(async () => await PlayTutorialVideo());
        }
    }
    public async UniTask PlayTutorialVideo()
    {
        if (videoPlayer == null || tutorialVideo == null)
        {
            Debug.LogWarning("[UIManager] Video player or tutorial video not assigned.");
            return;
        }
        ServiceLocator.Get<ISoundService>().Pause(SoundCategory.BGM);
        videoPlayer.gameObject.SetActive(true);
        await videoPlayer.PlayVideo(tutorialVideo);
        await UniTask.Delay(2000, ignoreTimeScale: true);
        videoPlayer.gameObject.SetActive(false);
        ServiceLocator.Get<ISoundService>().Resume(SoundCategory.BGM);
    }
    public bool HasTutorialVideo => videoPlayer != null && tutorialVideo != null;
}
