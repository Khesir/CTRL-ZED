using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BioChipsConversionSection : MonoBehaviour
{
    public TMP_Text biochipCount;
    private IBioChipService instance;
    public void Initialize(IBioChipService resourceManager)
    {
        instance = resourceManager;
        UpdateText();
        resourceManager.OnSpendBioChip += UpdateText;
    }

    private void UpdateText()
    {
        biochipCount.text = instance.GetBioChip().ToString();
    }
}
