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
        var manager = GameManager.Instance.PlayerManager;

        inventoryUI.Populate();
        characterCounter.Setup(manager.GetOwnedCharacters().Count);

        GameManager.Instance.PlayerManager.onInventoryChange += characterCounter.UpdateCounter;
        GameManager.Instance.PlayerManager.onInventoryChange += inventoryUI.RefreshUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerManager.onInventoryChange -= characterCounter.UpdateCounter;
        GameManager.Instance.PlayerManager.onInventoryChange -= inventoryUI.RefreshUI;
    }
}
