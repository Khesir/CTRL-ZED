using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Page3Resources : MonoBehaviour
{
    public TMP_Text charges;
    public Button lockResource;
    [Header("Section References")]
    public P3ResourceSection p3ResourceSection;
    public BioChipsConversionSection bioChipsConversionSection;
    void OnEnable()
    {
        var manager = GameManager.Instance.ResourceManager;
        p3ResourceSection.Initialize(manager);
        bioChipsConversionSection.Initialize(manager);
        charges.text = manager.GetRemainingCharge().ToString();
        lockResource.onClick.RemoveAllListeners();
        lockResource.onClick.AddListener(ActionButton);
    }
    private void ActionButton()
    {
        var manager = GameManager.Instance.ResourceManager;
        manager.SpendBioChips();
        charges.text = manager.GetRemainingCharge().ToString();
    }
}
