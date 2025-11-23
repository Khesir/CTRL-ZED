using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Display")]
    [SerializeField] private Image bannerImage;
    [SerializeField] private Image disabled;

    [Header("Modal")]
    [SerializeField] public LevelInformationModal modalComponent;

    private LevelData data;
    private Sprite normalBanner;
    private Sprite hoverBanner;

    public void Setup(LevelData data)
    {
        this.data = data;
        normalBanner = data.levelBanner;
        hoverBanner = data.hoverLevelBanner;

        // Set initial banner
        if (bannerImage != null)
            bannerImage.sprite = normalBanner;

        // Setup button click
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenModal);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bannerImage != null && hoverBanner != null)
            bannerImage.sprite = hoverBanner;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bannerImage != null && normalBanner != null)
            bannerImage.sprite = normalBanner;
    }

    private void OpenModal()
    {
        if (modalComponent != null)
        {
            modalComponent.data = data;
            modalComponent.Trigger();
        }
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_OnOpen);
    }
}
