using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSet", menuName = "Configs/Wave Set")]
public class WaveSet : ScriptableObject
{
    public List<WaveConfig> waves;
}
