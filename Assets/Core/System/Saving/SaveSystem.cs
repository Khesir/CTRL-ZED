using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public static class SaveSystem
{
    static public void SaveToSlot(int slot, SaveData data)
    {
        string path = Application.persistentDataPath + $"/slot{slot}.json";
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }
    static public SaveData LoadFromSlot(int slot)
    {
        string path = Application.persistentDataPath + $"/slot{slot}.json";
        if (File.Exists(path))
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        return null;
    }
}
