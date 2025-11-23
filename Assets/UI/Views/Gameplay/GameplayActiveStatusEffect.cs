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
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
