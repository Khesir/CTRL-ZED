using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Characters/Character")]
public class CharacterData : ScriptableObject
{
    public Sprite icon;
    public string className;
    public string charactername;
    public GameObject prefab;

    [Header("Stats")]
    public int baseAttack = 1;
    public int baseHealth;
    public int defense;
    public int dex;
    public int level;
    [Header("Shop Info")]
    public int price;
}
