using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsController : MonoBehaviour
{
    [Header("References")]
    public Transform content;
    public GameObject prefab;
    public TMP_Text className;
    public TMP_Text characterName;
    public Image characterIcon;
    public Image characterShip;
    public void Intialize(CharacterService data)
    {
        Debug.Log(data);
        Populate(data);
    }
    public void Populate(CharacterService data)
    {
        var character = data.GetInstance();
        characterName.text = character.name;
        className.text = $"{character.baseData.className} - Lvl {character.level}";
        characterIcon.sprite = character.baseData.icon;
        characterShip.sprite = character.baseData.ship;
        Clear();
        var statsMap = character.GetDeploymentCost();
        foreach (var instance in statsMap)
        {
            var statCard = Instantiate(prefab, content);
            var card = statCard.GetComponent<StatSlotUI>();
            card.Setup(instance);
        }
    }

    public void Clear()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
