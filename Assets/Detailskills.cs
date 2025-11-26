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
        // Safety check: don't initialize if object is being destroyed
        if (this == null || parentSlot == null) return;

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
        // Safety check: don't clear if object is being destroyed
        if (parentSlot == null) return;

        foreach (Transform child in parentSlot)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
