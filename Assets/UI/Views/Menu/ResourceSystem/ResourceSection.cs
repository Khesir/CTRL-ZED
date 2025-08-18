using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceSection : MonoBehaviour
{
    [SerializeField] private TMP_Text ResourceEnergy;
    [SerializeField] private TMP_Text ResourceFood;
    [SerializeField] private TMP_Text ResourceTechnology;
    [SerializeField] private TMP_Text ResourceIntelligence;
    private PlayerService service;
    public void Setup()
    {
        service = GameManager.Instance.PlayerManager.playerService;
        UpdateText();
        service.OnResourceChange += UpdateText;
    }
    public void OnDestroy()
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
