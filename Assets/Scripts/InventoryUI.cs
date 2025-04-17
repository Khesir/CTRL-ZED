using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform gridContainer;
    public GameObject slotPrefab;
    // Temporary
    public CharacterData[] characterTemplates;
    List<CharacterInstance> ownedCharacters = new();
    void OnEnable()
    {
        Debug.Log("Generating");
        GenerateCharacterData();
        Debug.Log("Populating");
        Populate();

    }

    void GenerateCharacterData()
    {
        foreach (var template in characterTemplates)
        {
            ownedCharacters.Add(new CharacterInstance(template));

            Debug.Log("Generated Character: " + template.charactername);

        }
    }

    public void Populate()
    {
        // Clear Old ones
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
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
