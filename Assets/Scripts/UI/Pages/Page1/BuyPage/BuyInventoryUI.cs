using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyInventoryUI : MonoBehaviour
{
  public Transform content;
  public GameObject slotPrefab;

  public void Populate(List<CharacterData> characterData)
  {
    // Clear Old Ones
    Clear();
    // Updated ones
    foreach (var instance in characterData)
    {
      var card = Instantiate(slotPrefab, content);
      Debug.Log("Card instantiated: " + card.name);

      var cardUI = card.GetComponent<BuySlotUI>();
      if (cardUI == null)
      {
        Debug.LogError("BuySlotUI is missing on the instantiated prefab!");
      }
      else
      {
        cardUI.Setup(instance);
      }
    }
  }
  public void Clear()
  {
    foreach (Transform child in content)
    {
      Destroy(child.gameObject);
    }
  }
}
