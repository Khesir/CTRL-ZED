using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAntivirusSelector : MonoBehaviour
{
    public GameObject antiVirusPrefab;
    public Transform content;

    public void Setup(UIAntivirusAbout target, List<AntiVirus> buffs)
    {
        Clear();
        Populate(target, buffs);
    }
    private void Populate(UIAntivirusAbout target, List<AntiVirus> buffs)
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
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
