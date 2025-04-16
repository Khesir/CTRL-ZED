using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform contentParent;
    public GameObject slotPrefab;

    void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var instance in CharacterInventory.Instance.ownedCharacters)
        {
            GameObject go = Instantiate(slotPrefab, contentParent);
            go.GetComponent<InventorySlotUI>().Setup(instance);
        }
    }
}
