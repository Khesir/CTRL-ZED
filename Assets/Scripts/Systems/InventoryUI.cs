using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform gridContainer;
    public GameObject slotPrefab;
    void OnEnable()
    {
        Populate();
    }


    public void Populate()
    {
        // Clear Old ones
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        List<CharacterInstance> ownedCharacters = GameManager.Instance.PlayerManager.playerData.ownedCharacters;
        // Add one card per character
        foreach (var instance in ownedCharacters)
        {
            var cardGO = Instantiate(slotPrefab, gridContainer);
            var card = cardGO.GetComponent<InventorySlotUI>();
            card.Setup(instance);
        }
    }

    public void RefreshUI()
    {
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        foreach (var instance in CharacterInventory.Instance.ownedCharacters)
        {
            GameObject go = Instantiate(slotPrefab, gridContainer);
            go.GetComponent<InventorySlotUI>().Setup(instance);
        }
    }
}
