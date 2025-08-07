using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListUI : MonoBehaviour
{
  public GameObject CharacterIcons;
  public GameObject hotbar1;
  public GameObject hotbar2;
  public GameObject hotbar3;
  public GameObject hotbar4;
  public void OnClose()
  {
    CharacterIcons.SetActive(true);
  }
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
        hotbar.GetComponent<CharacterDetails>().Initialize(characters[i]);
      }
      else
      {
        hotbar.SetActive(false);
      }
    }
    GameplayManager.Instance.followerManager.OnSwitch += UpdateHotbar1;
  }
  public void UpdateHotbar1()
  {
    var activePlayer = GameplayManager.Instance.followerManager.GetCurrentTargetBattleState();
    hotbar1.GetComponent<CharacterDetails>().Initialize(activePlayer);
  }
}
