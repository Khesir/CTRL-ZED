using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public string filePath;
    public async UniTask<PlayerData> Initialize()
    {
        Debug.Log($"Save Location {filePath}");
        // filePath = Application.persistentDataPath + "/playerData.json";
        return await LoadPlayerData();
    }
    public async UniTask<PlayerData> LoadPlayerData()
    {
        var data = new PlayerData();
        // Just simulating
        await UniTask.Yield();
        return data;
    }
}
