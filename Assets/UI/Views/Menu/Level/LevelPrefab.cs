using System.Collections.Generic;
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
    [Header("Level Requirements")]
    [SerializeField] private GameObject requirementContainer;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject clearCondition;
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
        Debug.Log($"level {isUnlocked}");
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
                requiredLevel.text = "No Requirement";
            }
            else
            {
                // set with this 727272 hex color
                bannerImage.color = new Color32(0x72, 0x72, 0x72, 255);
                requiredLevel.text = "OS Level Req. " + data.OsLevelRequirement;
            }
        }
        ClearRequirements(); // Clear game requirments gameobject
        InstantiateObjectLevelRequirement();
        clearCondition.GetComponent<TMP_Text>().text = data.clearCondition;
    }
    public void InstantiateObjectLevelRequirement()
    {
        List<CharacterRequirement> requirements = data.characterRequirements;
        Transform transform = requirementContainer.transform;

        if (requirements.Count < 1)
        {
            var obj = Instantiate(textObject, transform);
            obj.GetComponent<TMP_Text>().text = "No Specific Requirement";
        }
        foreach (CharacterRequirement requirement in requirements)
        {
            var obj = Instantiate(textObject, transform);
            obj.GetComponent<TMP_Text>().text = $"Req. {requirement.character.className} - LvL {requirement.levelRequirement}";
        }
    }
    public void ClearRequirements()
    {
        foreach (Transform child in requirementContainer.transform)
        {
            Destroy(child.gameObject); // destroys child of transform game object
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
