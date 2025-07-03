using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class P3ResourceSection : MonoBehaviour
{
    public TMP_Text food;
    public TMP_Text tech;
    public TMP_Text energy;
    public TMP_Text intel;
    public void Initialize(ResourceManager resourceManager)
    {
        food.text = resourceManager.GetFood().ToString();
        tech.text = resourceManager.GetTechnology().ToString();
        energy.text = resourceManager.GetEnergy().ToString();
        intel.text = resourceManager.GetIntelligence().ToString();
    }
}
