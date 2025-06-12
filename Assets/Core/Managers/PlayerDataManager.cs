using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public SaveData data; // OBject data base for now, will change to persistent database
    public string filePath;
    public async UniTask<SaveData> Initialize()
    {
        Debug.Log($"Save Location {filePath}");
        // filePath = Application.persistentDataPath + "/playerData.json";
        return await LoadPlayerData();
    }
    public async UniTask<SaveData> LoadPlayerData()
    {
        data = new SaveData();
        // Initial Base values
        // Just simulating
        await UniTask.Yield();
        return data;
    }
}
