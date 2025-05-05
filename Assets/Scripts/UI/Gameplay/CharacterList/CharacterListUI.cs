using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{
  public Transform content;
  public GameObject CharacterPrefab;

  public void Setup(List<CharacterData> characters)
  {
    foreach (var instance in characters)
    {
      var characterCard = Instantiate(CharacterPrefab, content);

      characterCard.GetComponent<GameplayCharacterCardDetails>();
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
