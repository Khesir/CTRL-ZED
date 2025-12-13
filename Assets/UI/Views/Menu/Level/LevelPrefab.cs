using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Display")]
    [SerializeField] private Image bannerImage;
    [SerializeField] private TMP_Text requiredLevel;
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

        // Check if level is unlocked
        bool isUnlocked = ServiceLocator.Get<ILevelManager>().IsLevelUnlocked(data);

        // Setup button click and interactability
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = isUnlocked;

            // Only add click listener if unlocked
            if (isUnlocked)
            {
                // set to white with a = 1 
                bannerImage.color = Color.white;
                button.onClick.AddListener(OpenModal);
                requiredLevel.text = "";
            }
            else
            {
                // set with this 727272 hex color
                bannerImage.color = new Color32(0x72, 0x72, 0x72, 255);
                requiredLevel.text = "OS Level Req. " + data.OsLevelRequirement;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var button = GetComponent<Button>();
        if (button != null && button.interactable && bannerImage != null && hoverBanner != null)
            bannerImage.sprite = hoverBanner;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var button = GetComponent<Button>();
        if (button != null && button.interactable && bannerImage != null && normalBanner != null)
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
