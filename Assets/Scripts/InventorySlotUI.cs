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
    public CharacterInstance instance;

    public void Setup(CharacterInstance character)
    {
        instance = character;
        className.text = character.template.className;
        icon.sprite = character.template.icon;
        nameText.text = character.template.charactername;
        level.text = character.level.ToString();

        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(OnActionButtonClicked);
        Debug.Log("Generated InvenetoryDaata");
    }

    private void OnActionButtonClicked()
    {
        Debug.Log("Clicked: " + instance.template.charactername);
    }
}
