using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootDropData
{
    public ItemConfig item;
    public int amount;
    [Range(0f, 1f)] public float dropChance = 1f;
}
