using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
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
    public DetailsController detailsController;
    public CharacterData instance;

    public void Setup(CharacterData character)
    {
        instance = character;
        className.text = character.baseData.className;
        icon.sprite = character.baseData.icon;
        nameText.text = character.name;
        level.text = character.level.ToString();

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnActionButtonClicked);
        Debug.Log("Generated InvenetoryDaata");
    }

    private void OnActionButtonClicked()
    {
        detailsController.Intialize(instance);
        Debug.Log("Clicked: " + instance.baseData.className);
    }
}
