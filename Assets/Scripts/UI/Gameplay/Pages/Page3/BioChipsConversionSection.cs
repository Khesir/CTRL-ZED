using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BioChipsConversionSection : MonoBehaviour
{
    public TMP_Text biochipCount;
    private ResourceManager instance;
    public void Initialize(ResourceManager resourceManager)
    {
        instance = resourceManager;
        UpdateText();
        resourceManager.onSpendBioChip += UpdateText;
    }

    private void UpdateText()
    {
        biochipCount.text = instance.GetBioChips().ToString();
    }
}
