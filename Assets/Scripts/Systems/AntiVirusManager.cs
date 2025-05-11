using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AntiVirusManager : MonoBehaviour
{
    public List<AntiVirus> antiVirusStage;
    public int level;
    public int maxLevel;
    public async UniTask Initialize(int level = -1)
    {
        this.level = level;
        maxLevel = antiVirusStage.Count;
        await UniTask.CompletedTask;
    }
    public void LevelUp()
    {
        level++;
    }
    public void SetLevel(int level)
    {
        if (this.level < level)
        {
            this.level = level;
        }
    }
}
