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
    public void Intialize(CharacterData data)
    {
        Debug.Log(data);
        Populate(data);
    }
    public void Populate(CharacterData data)
    {
        var character = data;
        characterName.text = character.name;
        className.text = $"{character.baseData.className} - Lvl {character.level}";
        characterIcon.sprite = character.baseData.icon;
        characterShip.sprite = character.baseData.ship;
        Clear();
        float multiplier = Mathf.Pow(1.2f, data.currentLevel - 1);

        var statsMap = new Dictionary<string, float>{
            {"Food", data.baseData.food * multiplier },
            {"Technology", data.baseData.technology * multiplier},
            {"Energy", data.baseData.energy * multiplier},
            {"Intelligence", data.baseData.intelligence* multiplier}
        };
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
