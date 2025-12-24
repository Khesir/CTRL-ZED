using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CityLevel", menuName = "GameplayLevels/City")]
public class LevelData : ScriptableObject
{
    [Header("Basic Info")]
    public Sprite levelBanner;
    public Sprite hoverLevelBanner;
    public Sprite levelInformation;
    public string levelID;
    public string levelName;
    public string objective;
    public string description;
    public string recommendation;
    public bool isCleared;

    [Header("Level Setup")]
    public WaveSet waveSet;
    public List<Material> background;
    [Header("Extra Unlock Conditions")]
    public int OsLevelRequirement;
    public List<CharacterRequirement> characterRequirements;
    public string clearCondition;
}
[System.Serializable]
public class CharacterRequirement
{
    public CharacterConfig character;
    public int levelRequirement;
}