using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text className;
    public TMP_Text nameText;
    public TMP_Text level;
    public CharacterService instance;
    public DraggableItem draggableItem;
    public Button actionButton;
    [Header("Dont touch below, Handled automatically")]
    public DetailsController detailsController;
    public GameObject noCharacterDetailsSection;
    public void Setup(CharacterService data)
    {
        var character = data.GetInstance();
        instance = data;
        className.text = character.baseData.className;
        nameText.text = character.name;
        level.text = $"Lvl. {character.level}";
        draggableItem.Setup(data, true);

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnActionButtonClicked);
    }

    private void OnActionButtonClicked()
    {
        // Just making sure that it renders the right one
        // By default no character Details selected, as this consider if no characters
        // Base case.
        detailsController.gameObject.SetActive(true);
        noCharacterDetailsSection.SetActive(false);

        detailsController.Intialize(instance);
    }
}
