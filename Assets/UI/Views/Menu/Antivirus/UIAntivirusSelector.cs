using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAntivirusSelector : MonoBehaviour
{
    public GameObject antiVirusPrefab;
    public Transform content;
    public UIAntivirusAbout target;
    public List<StatusEffectData> buffs;

    public void Setup(UIAntivirusAbout target, List<StatusEffectData> buffs)
    {
        this.target = target;
        this.buffs = buffs;
        Refresh();
        ServiceLocator.Get<IPlayerManager>().playerService.OnCoinsChange += Refresh;
    }
    public void OnDisable()
    {
        ServiceLocator.Get<IPlayerManager>().playerService.OnCoinsChange -= Refresh;
    }
    private void Refresh()
    {
        Clear();
        Populate(target, buffs);
    }
    private void Populate(UIAntivirusAbout target, List<StatusEffectData> buffs)
    {
        foreach (var effect in buffs)
        {
            var cardGo = Instantiate(antiVirusPrefab, content);
            var item = cardGo.GetComponent<VirusItem>();
            item.target = target;
            item.Setup(effect);
        }
    }
    public void Clear()
    {
        if (content.childCount > 0)
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
