using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySlotUI : MonoBehaviour
{
  public TMP_Text className;
  public Image icon;
  public Button actionButton;
  private CharacterData instance;
  public void Setup(CharacterData character)
  {
    instance = character;
    className.text = character.className;
    icon.sprite = character.icon;
    actionButton.GetComponentInChildren<TMP_Text>().text = character.price.ToString();

    actionButton.onClick.RemoveAllListeners();
    actionButton.onClick.AddListener(OnActionButtonClicked);
    Debug.Log("Generated Buy Slots");
  }

  private void OnActionButtonClicked()
  {
    var result = GameManager.Instance.PlayerManager.TryPurchase(instance);

    if (result.Success)
    {
      Debug.Log("Purchase successfull");
    }
    else
    {
      Debug.LogWarning(result.Message);
    }
  }
}
