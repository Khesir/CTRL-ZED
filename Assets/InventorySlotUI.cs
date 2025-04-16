using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public Text statstext;

    public CharacterInstance instance;

    public void Setup(CharacterInstance character)
    {
        instance = character;

        icon.sprite = character.template.icon;
        nameText.text = character.template.charactername;

        statstext.text = $"HP: {character.currentHealth}\n" +
            $"ATK: {character.GetAttack()}\n" +
            $"DEF: {character.template.defense}\n" +
            $"DEX: {character.template.dex}";
    }
}
