using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaveManager
{
    WaveService CurrentWave { get; }
    void Initialize();
    void SetWaveConfig(List<WaveConfig> waveConfigs);
    void StartNextWave();
    int GetWaveIndex();
    void ReportKill();
    void PauseWave(bool flag);
}
