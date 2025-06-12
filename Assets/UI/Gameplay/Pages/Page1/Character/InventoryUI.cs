using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform gridContainer;
    public GameObject slotPrefab;
    public DetailsController detailsController;

    public void Populate()
    {
        // Clear Old ones
        Clear();
        List<CharacterService> ownedCharacters = GameManager.Instance.CharacterManager.GetCharacters();
        // Add one card per character
        foreach (var instance in ownedCharacters)
        {
            var cardGO = Instantiate(slotPrefab, gridContainer);
            var card = cardGO.GetComponent<InventorySlotUI>();
            card.detailsController = detailsController;
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
