using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform gridContainer;
    public GameObject slotPrefab;
    public GameObject noCharacterAvailablePrefab;
    public DetailsController characterDetailsSection;
    public GameObject noCharacterDetailsSection;
    public void Populate()
    {
        Clear();
        List<CharacterService> ownedCharacters = GameManager.Instance.CharacterManager.GetCharacters();
        if (ownedCharacters.Count < 1)
        {
            Instantiate(noCharacterAvailablePrefab, gridContainer).SetActive(true);
            return;
        }
        foreach (var instance in ownedCharacters)
        {
            var cardGO = Instantiate(slotPrefab, gridContainer);
            var card = cardGO.GetComponent<InventorySlotUI>();
            card.detailsController = characterDetailsSection;
            card.noCharacterDetailsSection = noCharacterDetailsSection;
            card.Setup(instance);
        }
    }

    public void Clear()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshUI()
    {
        Populate();
    }
}
