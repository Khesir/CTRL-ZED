using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
  public string teamName;
  public bool isActiveTeam;
  public List<CharacterData> characters = new();
  public int maxSize = 4;
  public string teamID = "Not Set";
  public Team(string name)
  {
    teamName = name;
    for (int i = 0; i < maxSize; i++)
      characters.Add(null);
  }
}
