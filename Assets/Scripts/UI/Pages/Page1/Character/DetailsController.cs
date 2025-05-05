using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StatsUI statsUI;

    public void Intialize(CharacterService data)
    {
        gameObject.SetActive(true);
        statsUI.Populate(data);
    }
}
