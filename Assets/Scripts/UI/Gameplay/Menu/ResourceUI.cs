using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{

    public TMP_Text ResourceFood;

    public TMP_Text ResourceTechnology;

    public TMP_Text ResourceEnergy;

    public TMP_Text ResourceIntelligence;

    public TMP_Text ResourceCoins;
    public void Setup()
    {
        ResourceManager manager = GameManager.Instance.ResourceManager;
        ResourceFood.text = manager.GetFood().ToString();
        ResourceTechnology.text = manager.GetTechnology().ToString();
        ResourceEnergy.text = manager.GetEnergy().ToString();
        ResourceIntelligence.text = manager.GetIntelligence().ToString();
        ResourceCoins.text = manager.GetCoins().ToString();

    }
}
