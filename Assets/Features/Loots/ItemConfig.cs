using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Food,
    Technology,
    Energy,
    Intelligence,
    Coins
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Configs/Item")]
public class ItemConfig : ScriptableObject
{
    public string id;
    public ItemType itemType;
    public Sprite icon;
    public string displayName;
    public string description;
}
