using UnityEngine;

public class Page1Controller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CharacterCounter characterCounter;

    private ICharacterManager _characterManager;

    private void OnEnable()
    {
        _characterManager = ServiceLocator.Get<ICharacterManager>();

        inventoryUI.Populate();
        characterCounter.Setup(_characterManager.GetCharacters().Count);

        _characterManager.onInventoryChange += characterCounter.UpdateCounter;
        _characterManager.onInventoryChange += inventoryUI.RefreshUI;
    }

    private void OnDisable()
    {
        _characterManager.onInventoryChange -= characterCounter.UpdateCounter;
        _characterManager.onInventoryChange -= inventoryUI.RefreshUI;
    }
}
