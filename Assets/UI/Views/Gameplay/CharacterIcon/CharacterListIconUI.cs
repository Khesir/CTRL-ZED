using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListIconUI : MonoBehaviour
{
  public GameObject hotbar1;
  public GameObject hotbar2;
  public GameObject hotbar3;
  public GameObject hotbar4;
  public void Initialize(List<CharacterBattleState> characters)
  {
    var hotbars = new List<GameObject>
        {
            hotbar1,
            hotbar2,
            hotbar3,
            hotbar4
        };

    for (int i = 0; i < hotbars.Count; i++)
    {
      var hotbar = hotbars[i];

      if (characters[i] != null)
      {
        hotbar.SetActive(true);
        hotbar.GetComponent<CharacterDetailsIcon>().Initialize(characters[i], i);
      }
      else
      {
        hotbar.SetActive(false);
      }
    }
  }
}
