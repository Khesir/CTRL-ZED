using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public Transform content;
    public GameObject prefab;
    public TMP_Text className;
    public TMP_Text characterName;
    public void Populate(CharacterService data)
    {
        var character = data.GetInstance();
        characterName.text = character.name;
        className.text = character.baseData.className;

        Clear();
        var statsMap = character.GetStatMap();
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
    // public void RefreshUI()
    // {
    //     Populate();
    // 
}
