using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CharacterData
{
    public string id;
    public CharacterConfig baseData;

    public float maxHealth;
    public string name;
    public int level;

    public int currentLevel;
    public int experience;
    public List<int> assignedTeam = new();
}
