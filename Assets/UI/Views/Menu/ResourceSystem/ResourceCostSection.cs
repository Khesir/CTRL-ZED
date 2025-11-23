using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceCostSection : MonoBehaviour
{
    [SerializeField] private TMP_Text overallCost;
    private PlayerService service;
    public void Setup()
    {
        service = ServiceLocator.Get<IPlayerManager>().playerService;
        UpdateText();
        service.OnResourceChange += UpdateText;
    }
    public void OnDisable()
    {
        service.OnResourceChange -= UpdateText;
    }
    private void UpdateText()
    {
        var x = service.GetResourceChargePerDrives();
        overallCost.text = $"{x.food} Food \n{x.technology} Tech \n{x.energy} Energy \n{x.intelligence} Intelligence";
    }
}
