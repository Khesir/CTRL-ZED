using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public int activeLevel;
  public int currentLevelIndex = 0;
  public List<WaveListWrapper> levels;
}
[System.Serializable]
public class WaveListWrapper
{
  public List<Wave> waves;
  public List<Material> sprites;
  public Sprite icon;
}