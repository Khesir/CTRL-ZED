using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text ResourceEnergy;
    public TMP_Text ResourceFood;
    public TMP_Text ResourceTechnology;
    public TMP_Text ResourceIntelligence;
    private PlayerService service;
    public void Setup()
    {
        service = GameManager.Instance.PlayerManager.playerService;
        UpdateText();
        service.OnResourceChange += UpdateText;
    }
    public void OnDisable()
    {
        service.OnResourceChange -= UpdateText;
    }
    private void UpdateText()
    {
        ResourceFood.text = service.GetFood().ToString();
        ResourceTechnology.text = service.GetTechnology().ToString();
        ResourceEnergy.text = service.GetEnergy().ToString();
        ResourceIntelligence.text = service.GetIntelligence().ToString();
    }
}
