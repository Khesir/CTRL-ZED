using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page1Controller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CharacterCounter characterCounter;

    private void OnEnable()
    {
        inventoryUI.Populate();
        characterCounter.Setup(GameManager.Instance.CharacterManager.GetCharacters().Count);

        GameManager.Instance.CharacterManager.onInventoryChange += characterCounter.UpdateCounter;
        GameManager.Instance.CharacterManager.onInventoryChange += inventoryUI.RefreshUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.CharacterManager.onInventoryChange -= characterCounter.UpdateCounter;
        GameManager.Instance.CharacterManager.onInventoryChange -= inventoryUI.RefreshUI;
    }
}
