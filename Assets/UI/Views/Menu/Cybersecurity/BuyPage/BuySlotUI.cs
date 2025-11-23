using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class BuySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [Header("Display")]
  [SerializeField] private TMP_Text characterName;
  [SerializeField] private Image fallbackIcon;

  [Header("Video Preview")]
  [SerializeField] private VideoPlayer videoPlayer;
  [SerializeField] private RawImage videoDisplay;

  [Header("Purchase")]
  [SerializeField] private Button buyButton;
  [SerializeField] private TMP_Text priceText;

  [Header("Hover Effect")]
  [SerializeField] private float hoverScale = 1.1f;
  [SerializeField] private float scaleSpeed = 10f;

  private CharacterConfig instance;
  private RenderTexture renderTexture;
  private Vector3 originalScale;
  private Vector3 targetScale;
  private bool isHovered;

  private void Awake()
  {
    originalScale = transform.localScale;
    targetScale = originalScale;

    // Start with both hidden to avoid white flash
    videoDisplay.gameObject.SetActive(false);
    fallbackIcon.gameObject.SetActive(false);
  }

  private void Update()
  {
    // Smooth scale transition
    if (transform.localScale != targetScale)
    {
      transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }
  }

  public void Setup(CharacterConfig character)
  {
    instance = character;

    // Set name
    if (characterName != null)
      characterName.text = character.className;

    // Set price
    if (priceText != null)
      priceText.text = character.price.ToString();
    else if (buyButton != null)
      buyButton.GetComponentInChildren<TMP_Text>().text = character.price.ToString();

    // Setup video or fallback
    SetupPreview(character);

    // Setup buy button
    if (buyButton != null)
    {
      buyButton.onClick.RemoveAllListeners();
      buyButton.onClick.AddListener(OnBuyClicked);
    }

    Debug.Log($"[BuySlotUI] Setup complete for {character.className}");
  }

  private void SetupPreview(CharacterConfig character)
  {

    bool hasVideo = character.buyIcon != null && videoPlayer != null && videoDisplay != null;

    Debug.Log($"[BuySlotUI] {character.className} - hasVideo: {hasVideo}, buyIcon: {character.buyIcon != null}, videoPlayer: {videoPlayer != null}, videoDisplay: {videoDisplay != null}");

    if (hasVideo)
    {
      // Setup video - fallback stays hidden
      videoDisplay.gameObject.SetActive(true);
      SetupVideo(character.buyIcon);
      Debug.Log($"[BuySlotUI] {character.className} - Showing video, fallback active: {fallbackIcon?.gameObject.activeSelf}");
    }
    else if (character.buyIconFallBack != null)
    {
      fallbackIcon.gameObject.SetActive(true);
    }
    // If neither exists, both stay hidden
  }

  private void SetupVideo(VideoClip clip)
  {
    // Create render texture
    renderTexture = new RenderTexture((int)clip.width, (int)clip.height, 0);
    renderTexture.Create();

    videoPlayer.targetTexture = renderTexture;
    videoDisplay.texture = renderTexture;

    videoPlayer.source = VideoSource.VideoClip;
    videoPlayer.clip = clip;
    videoPlayer.isLooping = true;
    videoPlayer.playOnAwake = false;
    videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

    // Ensure video is stopped - will play on hover
    videoPlayer.Stop();
  }

  private void OnBuyClicked()
  {
    var playerService = ServiceLocator.Get<IPlayerManager>()?.playerService;
    if (playerService == null)
    {
      Debug.LogError("[BuySlotUI] PlayerService is null!");
      return;
    }

    var result = playerService.SpendCoins(instance.price);

    if (result)
    {
      ServiceLocator.Get<ICharacterManager>().CreateCharacter(instance);
      Debug.Log($"[BuySlotUI] Purchase successful: {instance.className}");
      ServiceLocator.Get<ISoundService>().Play(SoundCategory.Coins, SoundType.Coins_spend);
    }
    else
    {
      Debug.LogWarning("[BuySlotUI] Not enough coins");
      ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_Error);
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    isHovered = true;
    // targetScale = originalScale * hoverScale;

    if (videoPlayer != null && videoPlayer.clip != null)
    {
      videoPlayer.Play();
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    isHovered = false;
    targetScale = originalScale;

    if (videoPlayer != null)
    {
      videoPlayer.Pause();
      videoPlayer.frame = 0; // Reset to start
    }
  }

  private void OnDisable()
  {
    if (videoPlayer != null)
      videoPlayer.Stop();
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
