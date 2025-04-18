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
    public void Populate(CharacterInstance data)
    {
        Clear();
        characterName.text = data.name;
        className.text = data.template.className;

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
    // }
    public void Setup(PlayerData data, int level)
    {

    }
}
