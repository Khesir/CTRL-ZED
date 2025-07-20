using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CityLevel", menuName = "GameplayLevels/City")]
public class LevelData : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite levelIcon;
    public string levelID;
    public string levelName;
    public string objective;
    public string description;
    public string recommendation;
    public bool isCleared;

    [Header("Level Setup")]
    public List<Wave> waves;
    public List<Material> background;
}
