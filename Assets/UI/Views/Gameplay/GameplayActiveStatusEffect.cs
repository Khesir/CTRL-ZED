using System.Collections;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayActiveStatusEffect : MonoBehaviour
{
    public GameObject statusEffectPrefab;
    public void Setup()
    {
        UpdateUI();
        ServiceLocator.Get<IStatusEffectManager>().onBuffChange += UpdateUI;
    }
    private void UpdateUI()
    {
        // Safety check: don't update if object is being destroyed
        if (this == null || gameObject == null) return;

        Clear();
        var activeBuffs = ServiceLocator.Get<IStatusEffectManager>().activeBuffs;
        foreach (var buff in activeBuffs)
        {
            var go = Instantiate(statusEffectPrefab, this.gameObject.transform);
            var img = go.GetComponent<Image>();
            img.sprite = buff.data.icon;
        }
    }
    public void OnDestroy()
    {
        ServiceLocator.Get<IStatusEffectManager>().onBuffChange -= UpdateUI;
    }
    private void Clear()
    {
        // Safety check: don't clear if object is being destroyed
        if (this == null || gameObject == null) return;

        foreach (Transform child in gameObject.transform)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
