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
}
