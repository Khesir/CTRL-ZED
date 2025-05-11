using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text className;
    public TMP_Text nameText;
    public TMP_Text statstext;
    public TMP_Text level;
    public Button actionButton;
    public CharacterService instance;
    public DraggableItem draggableItem;
    public DetailsController detailsController;
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
        detailsController.Intialize(instance);
    }
}
