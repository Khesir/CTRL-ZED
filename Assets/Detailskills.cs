using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detailskills : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform parentSlot;
    public void Initialize(List<SkillConfig> skillConfigs)
    {
        Clear();
        foreach (var skill in skillConfigs)
        {
            var go = Instantiate(prefab, parentSlot);
            var img = go.GetComponent<Image>();
            img.sprite = skill.icon;
        }
    }
    public void Clear()
    {
        foreach (Transform child in parentSlot)
        {
            Destroy(child.gameObject);
        }
    }
}
